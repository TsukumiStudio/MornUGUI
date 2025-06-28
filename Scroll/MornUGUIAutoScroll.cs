using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace MornUGUI
{
    /// <summary>選択されたUI要素が自動的にスクロールビュー内に表示されるようにするコンポーネント</summary>
    [RequireComponent(typeof(ScrollRect))]
    public sealed class MornUGUIAutoScroll : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        
        private RectTransform _viewportRect;
        private RectTransform _contentRect;
        private readonly Vector3[] _cornersCache = new Vector3[4];

        private void Awake()
        {
            if (_scrollRect == null)
            {
                _scrollRect = GetComponent<ScrollRect>();
            }

            CacheComponents();
            SubscribeToSelectables();
        }

        private void Reset()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

        private void CacheComponents()
        {
            _contentRect = _scrollRect.content;
            _viewportRect = _scrollRect.viewport != null 
                ? _scrollRect.viewport 
                : _scrollRect.GetComponent<RectTransform>();
        }

        private void SubscribeToSelectables()
        {
            var selectables = _scrollRect.GetComponentsInChildren<Selectable>(true);
            
            foreach (var selectable in selectables)
            {
                selectable.OnSelectAsObservable()
                    .Subscribe(_ => ScrollToElement(selectable))
                    .AddTo(this);
            }
        }

        private void ScrollToElement(Selectable selectable)
        {
            if (!IsValid())
            {
                return;
            }

            var targetRect = selectable.GetComponent<RectTransform>();
            if (targetRect == null)
            {
                return;
            }

            var targetBounds = GetBoundsInContentSpace(targetRect);
            var viewportBounds = GetBoundsInContentSpace(_viewportRect);

            if (IsFullyVisible(targetBounds, viewportBounds))
            {
                return;
            }

            var newPosition = CalculateScrollPosition(targetBounds, viewportBounds);
            ApplyScrollPosition(newPosition);
        }

        private bool IsValid()
        {
            return _scrollRect != null && _contentRect != null && _viewportRect != null;
        }

        private Rect GetBoundsInContentSpace(RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(_cornersCache);
            
            for (var i = 0; i < 4; i++)
            {
                _cornersCache[i] = _contentRect.InverseTransformPoint(_cornersCache[i]);
            }

            var minX = Mathf.Min(_cornersCache[0].x, _cornersCache[1].x, _cornersCache[2].x, _cornersCache[3].x);
            var maxX = Mathf.Max(_cornersCache[0].x, _cornersCache[1].x, _cornersCache[2].x, _cornersCache[3].x);
            var minY = Mathf.Min(_cornersCache[0].y, _cornersCache[1].y, _cornersCache[2].y, _cornersCache[3].y);
            var maxY = Mathf.Max(_cornersCache[0].y, _cornersCache[1].y, _cornersCache[2].y, _cornersCache[3].y);
            
            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }

        private bool IsFullyVisible(Rect targetBounds, Rect viewportBounds)
        {
            var targetLeft = targetBounds.xMin;
            var targetRight = targetBounds.xMax;
            var targetTop = targetBounds.yMax;
            var targetBottom = targetBounds.yMin;
            
            var viewportLeft = viewportBounds.xMin;
            var viewportRight = viewportBounds.xMax;
            var viewportTop = viewportBounds.yMax;
            var viewportBottom = viewportBounds.yMin;
            
            var isVerticallyVisible = targetTop <= viewportTop && targetBottom >= viewportBottom;
            var isHorizontallyVisible = targetLeft >= viewportLeft && targetRight <= viewportRight;
            
            return isVerticallyVisible && isHorizontallyVisible;
        }

        private Vector2 CalculateScrollPosition(Rect targetBounds, Rect viewportBounds)
        {
            var currentPosition = _contentRect.anchoredPosition;
            var newPosition = currentPosition;
            
            // 垂直方向の調整
            if (_scrollRect.vertical)
            {
                var targetTop = targetBounds.yMax;
                var targetBottom = targetBounds.yMin;
                var viewportTop = viewportBounds.yMax;
                var viewportBottom = viewportBounds.yMin;
                
                if (targetBottom < viewportBottom)
                {
                    newPosition.y = currentPosition.y + (viewportBottom - targetBottom);
                }
                else if (targetTop > viewportTop)
                {
                    newPosition.y = currentPosition.y - (targetTop - viewportTop);
                }
            }
            
            // 水平方向の調整
            if (_scrollRect.horizontal)
            {
                var targetLeft = targetBounds.xMin;
                var targetRight = targetBounds.xMax;
                var viewportLeft = viewportBounds.xMin;
                var viewportRight = viewportBounds.xMax;
                
                if (targetLeft < viewportLeft)
                {
                    newPosition.x = currentPosition.x - (viewportLeft - targetLeft);
                }
                else if (targetRight > viewportRight)
                {
                    newPosition.x = currentPosition.x + (targetRight - viewportRight);
                }
            }
            
            return newPosition;
        }

        private void ApplyScrollPosition(Vector2 newPosition)
        {
            var viewportSize = _viewportRect.rect.size;
            var contentSize = _contentRect.rect.size;
            
            // 垂直方向の制限
            if (_scrollRect.vertical)
            {
                var maxScrollY = Mathf.Max(0, contentSize.y - viewportSize.y);
                newPosition.y = Mathf.Clamp(newPosition.y, 0, maxScrollY);
            }
            else
            {
                newPosition.y = _contentRect.anchoredPosition.y;
            }
            
            // 水平方向の制限
            if (_scrollRect.horizontal)
            {
                var maxScrollX = Mathf.Max(0, contentSize.x - viewportSize.x);
                newPosition.x = Mathf.Clamp(newPosition.x, -maxScrollX, 0);
            }
            else
            {
                newPosition.x = _contentRect.anchoredPosition.x;
            }
            
            _contentRect.anchoredPosition = newPosition;
        }
    }
}