using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIScrollbarActiveModule : MornUGUIScrollbarModuleBase
    {
        [SerializeField] private Selectable _upperArrow;
        [SerializeField] private Selectable _bottomArrow;
        [SerializeField] private Selectable _leftArrow;
        [SerializeField] private Selectable _rightArrow;
        
        private const float Threshold = 0.01f;

        public override void OnEnable(MornUGUIScrollbar parent)
        {
            UpdateArrow(parent);
        }

        public override void OnDisable(MornUGUIScrollbar parent)
        {
            if (_upperArrow != null)
            {
                _upperArrow.gameObject.SetActive(false);
            }

            if (_bottomArrow != null)
            {
                _bottomArrow.gameObject.SetActive(false);
            }
            
            if (_leftArrow != null)
            {
                _leftArrow.gameObject.SetActive(false);
            }

            if (_rightArrow != null)
            {
                _rightArrow.gameObject.SetActive(false);
            }
        }

        public override void Awake(MornUGUIScrollbar parent)
        {
            if (_upperArrow != null)
            {
                _upperArrow.OnSubmitAsObservable().Subscribe(_ => parent.ToUp()).AddTo(parent);
            }

            if (_bottomArrow != null)
            {
                _bottomArrow.OnSubmitAsObservable().Subscribe(_ => parent.ToBottom()).AddTo(parent);
            }
            
            if (_leftArrow != null)
            {
                _leftArrow.OnSubmitAsObservable().Subscribe(_ => parent.ToUp()).AddTo(parent);
            }

            if (_rightArrow != null)
            {
                _rightArrow.OnSubmitAsObservable().Subscribe(_ => parent.ToBottom()).AddTo(parent);
            }

            UpdateArrow(parent);
        }

        public override void OnValueChanged(MornUGUIScrollbar parent)
        {
            // UIのリビルドループ中の場合は次フレームに遅延
            Observable.NextFrame()
                .Subscribe(_ => UpdateArrow(parent))
                .AddTo(parent);
        }

        private void UpdateArrow(MornUGUIScrollbar parent)
        {
            var canMove = parent.Size < 1 && parent.gameObject.activeSelf;
            var isVertical = parent.Direction == Scrollbar.Direction.BottomToTop || parent.Direction == Scrollbar.Direction.TopToBottom;
            var isHorizontal = parent.Direction == Scrollbar.Direction.LeftToRight || parent.Direction == Scrollbar.Direction.RightToLeft;
            
            // 垂直方向の矢印
            if (isVertical)
            {
                var canUpper = canMove && !(Mathf.Abs(parent.Value - 1) < Threshold);
                var canBottom = canMove && !(Mathf.Abs(parent.Value) < Threshold);
                
                if (_upperArrow != null)
                {
                    _upperArrow.gameObject.SetActive(canUpper);
                }

                if (_bottomArrow != null)
                {
                    _bottomArrow.gameObject.SetActive(canBottom);
                }
                
                // 水平方向の矢印は非表示
                if (_leftArrow != null)
                {
                    _leftArrow.gameObject.SetActive(false);
                }

                if (_rightArrow != null)
                {
                    _rightArrow.gameObject.SetActive(false);
                }
            }
            // 水平方向の矢印
            else if (isHorizontal)
            {
                var isLeftToRight = parent.Direction == Scrollbar.Direction.LeftToRight;
                var canLeft = canMove && !(Mathf.Abs(parent.Value - (isLeftToRight ? 0 : 1)) < Threshold);
                var canRight = canMove && !(Mathf.Abs(parent.Value - (isLeftToRight ? 1 : 0)) < Threshold);
                
                if (_leftArrow != null)
                {
                    _leftArrow.gameObject.SetActive(canLeft);
                }

                if (_rightArrow != null)
                {
                    _rightArrow.gameObject.SetActive(canRight);
                }
                
                // 垂直方向の矢印は非表示
                if (_upperArrow != null)
                {
                    _upperArrow.gameObject.SetActive(false);
                }

                if (_bottomArrow != null)
                {
                    _bottomArrow.gameObject.SetActive(false);
                }
            }
        }
    }
}