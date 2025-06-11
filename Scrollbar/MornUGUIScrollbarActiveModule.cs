using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace MornUGUI
{
    [Serializable]
    internal sealed class MornUGUIScrollbarActiveModule : MornUGUIScrollbarModuleBase
    {
        [SerializeField] private Selectable _upperArrow;
        [SerializeField] private Selectable _bottomArrow;

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

            UpdateArrow(parent);
        }

        public override void OnValueChanged(MornUGUIScrollbar parent)
        {
            UpdateArrow(parent);
        }

        private void UpdateArrow(MornUGUIScrollbar parent)
        {
            var canMove = parent.Size < 1;
            var canUpper = canMove && !(Mathf.Abs(parent.Value - 1) < 0.001f);
            var canBottom = canMove && !(Mathf.Abs(parent.Value) < 0.001f);
            if (_upperArrow != null)
            {
                _upperArrow.gameObject.SetActive(canUpper);
            }

            if (_bottomArrow != null)
            {
                _bottomArrow.gameObject.SetActive(canBottom);
            }
        }
    }
}