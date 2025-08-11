using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace MornUGUI
{
    /// <summary>選択されたUI要素を自動的にスクロールビュー内に表示するモジュール</summary>
    [Serializable]
    internal sealed class MornUGUIScrollRectAutoScrollModule : MornUGUIScrollRectModuleBase
    {
        [SerializeField] private bool _isActive = true;
        [SerializeField] private float _scrollDuration = 0.3f;
        [SerializeField] private AnimationCurve _scrollCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        private readonly Vector3[] _cornersCache = new Vector3[4];
        private IDisposable _scrollAnimation;
        private Selectable _nextTarget;
        private Selectable _lastScrollTo;
        private int _lastChildCount;

        public override void Awake(MornUGUIScrollRect parent)
        {
            if (!_isActive)
            {
                return;
            }

            var selectables = parent.ScrollRect.GetComponentsInChildren<Selectable>(true);
            foreach (var selectable in selectables)
            {
                selectable.OnSelectAsObservable().Subscribe(_ => 
                {
                    _nextTarget = selectable;
                }).AddTo(parent);
            }
        }
        
        public override void OnUpdate(MornUGUIScrollRect parent)
        {
            if (!_isActive)
            {
                return;
            }
            
            // 子要素数の変化を検知
            var currentChildCount = GetActiveChildCount(parent);
            if (currentChildCount != _lastChildCount)
            {
                _lastChildCount = currentChildCount;
                _lastScrollTo = null; // 子要素数が変わったら最後にスクロールした要素をリセット
            }
            
            if (_nextTarget != null && _nextTarget.gameObject.activeInHierarchy && _nextTarget != _lastScrollTo)
            {
                ScrollToElement(parent, _nextTarget);
            }
        }
        
        private int GetActiveChildCount(MornUGUIScrollRect parent)
        {
            var content = parent.Content;
            var count = 0;
            for (var i = 0; i < content.childCount; i++)
            {
                var child = content.GetChild(i);
                if (child.gameObject.activeInHierarchy)
                {
                    count++;
                }
            }

            return count;
        }

        public override void OnDestroy(MornUGUIScrollRect scrollRect)
        {
            _scrollAnimation?.Dispose();
            _nextTarget = null;
        }

        private void ScrollToElement(MornUGUIScrollRect parent, Selectable target)
        {
            if (!_isActive)
            {
                return;
            }

            var targetRect = target.GetComponent<RectTransform>();
            if (targetRect == null)
            {
                return;
            }

            var targetBounds = GetBoundsInContentSpace(parent, targetRect);
            var viewportBounds = GetBoundsInContentSpace(parent, parent.ViewPort);
            if (IsFullyVisible(parent.ScrollRect, targetBounds, viewportBounds))
            {
                return;
            }

            var targetPosition = CalculateScrollPosition(parent, targetBounds, viewportBounds);
            if (_scrollDuration > 0)
            {
                AnimateScrollPosition(parent, targetPosition);
            }
            else
            {
                ApplyScrollPosition(parent, targetPosition);
            }
            _lastScrollTo = target;
        }

        private Rect GetBoundsInContentSpace(MornUGUIScrollRect parent, RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(_cornersCache);
            for (var i = 0; i < 4; i++)
            {
                _cornersCache[i] = parent.Content.InverseTransformPoint(_cornersCache[i]);
            }

            var minX = Mathf.Min(_cornersCache[0].x, _cornersCache[1].x, _cornersCache[2].x, _cornersCache[3].x);
            var maxX = Mathf.Max(_cornersCache[0].x, _cornersCache[1].x, _cornersCache[2].x, _cornersCache[3].x);
            var minY = Mathf.Min(_cornersCache[0].y, _cornersCache[1].y, _cornersCache[2].y, _cornersCache[3].y);
            var maxY = Mathf.Max(_cornersCache[0].y, _cornersCache[1].y, _cornersCache[2].y, _cornersCache[3].y);
            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }

        private bool IsFullyVisible(ScrollRect scrollRect, Rect targetBounds, Rect viewportBounds)
        {
            var horizontallyVisible = !scrollRect.horizontal
                                      || (targetBounds.xMin >= viewportBounds.xMin
                                          && targetBounds.xMax <= viewportBounds.xMax);
            var verticallyVisible = !scrollRect.vertical
                                    || (targetBounds.yMin >= viewportBounds.yMin
                                        && targetBounds.yMax <= viewportBounds.yMax);
            return horizontallyVisible && verticallyVisible;
        }

        private Vector2 CalculateScrollPosition(MornUGUIScrollRect parent, Rect targetBounds, Rect viewportBounds)
        {
            var currentPosition = parent.Content.anchoredPosition;
            var newPosition = currentPosition;
            if (parent.IsVertical)
            {
                if (targetBounds.yMin < viewportBounds.yMin)
                {
                    newPosition.y = currentPosition.y + (viewportBounds.yMin - targetBounds.yMin);
                }
                else if (targetBounds.yMax > viewportBounds.yMax)
                {
                    newPosition.y = currentPosition.y - (targetBounds.yMax - viewportBounds.yMax);
                }
            }

            if (parent.IsHorizontal)
            {
                if (targetBounds.xMin < viewportBounds.xMin)
                {
                    newPosition.x = currentPosition.x + (viewportBounds.xMin - targetBounds.xMin);
                }
                else if (targetBounds.xMax > viewportBounds.xMax)
                {
                    newPosition.x = currentPosition.x - (targetBounds.xMax - viewportBounds.xMax);
                }
            }

            return newPosition;
        }

        private void AnimateScrollPosition(MornUGUIScrollRect parent, Vector2 targetPosition)
        {
            _scrollAnimation?.Dispose();
            var startPosition = parent.Content.anchoredPosition;
            _scrollAnimation = Observable.EveryUpdate().Select(_ => Time.deltaTime)
                                         .Scan(0f, (acc, delta) => acc + delta)
                                         .TakeWhile(time => time < _scrollDuration)
                                         .Select(time => _scrollCurve.Evaluate(time / _scrollDuration)).Subscribe(
                                              t =>
                                              {
                                                  var position = Vector2.Lerp(startPosition, targetPosition, t);
                                                  ApplyScrollPosition(parent, position);
                                              }).AddTo(parent);
        }

        private void ApplyScrollPosition(MornUGUIScrollRect parent, Vector2 newPosition)
        {
            var viewportSize = parent.ViewPort.rect.size;
            var contentSize = parent.Content.rect.size;
            if (parent.IsVertical)
            {
                var maxScrollY = Mathf.Max(0, contentSize.y - viewportSize.y);
                newPosition.y = Mathf.Clamp(newPosition.y, 0, maxScrollY);
            }
            else
            {
                newPosition.y = parent.Content.anchoredPosition.y;
            }

            if (parent.IsHorizontal)
            {
                var maxScrollX = Mathf.Max(0, contentSize.x - viewportSize.x);
                newPosition.x = Mathf.Clamp(newPosition.x, -maxScrollX, 0);
            }
            else
            {
                newPosition.x = parent.Content.anchoredPosition.x;
            }

            parent.Content.anchoredPosition = newPosition;
        }
    }
}