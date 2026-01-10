using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUISliderNavigationModule : MornUGUISliderModuleBase
    {
        [SerializeField] private Selectable _up;
        [SerializeField] private Selectable _down;
        [SerializeField] private Selectable _left;
        [SerializeField] private Selectable _right;
        private float _lastValue;

        public override void OnValueChanged(MornUGUISlider parent)
        {
            _lastValue = parent.Value;
        }

        public override void OnMove(MornUGUISlider parent, AxisEventData axisEventData)
        {
            switch (parent.Direction)
            {
                case Slider.Direction.LeftToRight:
                    MoveHorizontal(parent, axisEventData, true);
                    break;
                case Slider.Direction.RightToLeft:
                    MoveHorizontal(parent, axisEventData, false);
                    break;
                case Slider.Direction.BottomToTop:
                    MoveVertical(parent, axisEventData, true);
                    break;
                case Slider.Direction.TopToBottom:
                    MoveVertical(parent, axisEventData, false);
                    break;
            }
        }

        private void MoveHorizontal(MornUGUISlider parent, AxisEventData axisEventData, bool isToRight)
        {
            var toLeft = axisEventData.moveDir == MoveDirection.Left && _left != null;
            var toRight = axisEventData.moveDir == MoveDirection.Right && _right != null;
            if (toLeft || toRight)
            {
                var atMin = Mathf.Approximately(parent.Value, parent.MinValue);
                var atMax = Mathf.Approximately(parent.Value, parent.MaxValue);
                
                var atLeft = isToRight ? atMin : atMax;
                var atRight = isToRight ? atMax : atMin;
                
                var canLeft = toLeft && atLeft;
                var canRight = toRight && atRight;
                if ((canLeft || canRight) && Mathf.Approximately(parent.Value, _lastValue))
                {
                    var nextSelectable = canLeft ? _left : _right;
                    EventSystem.current.SetSelectedGameObject(nextSelectable.gameObject);
                }
            }
        }

        private void MoveVertical(MornUGUISlider parent, AxisEventData axisEventData, bool isTop)
        {
            var toUp = axisEventData.moveDir == MoveDirection.Up && _up != null;
            var toDown = axisEventData.moveDir == MoveDirection.Down && _down != null;
            if (toUp || toDown)
            {
                var atMin = Mathf.Approximately(parent.Value, parent.MinValue);
                var atMax = Mathf.Approximately(parent.Value, parent.MaxValue);
                
                var atDown = isTop ? atMin : atMax;
                var atUp = isTop ? atMax : atMin;
                
                var canUp = toUp && atUp;
                var canDown = toDown && atDown;
                if ((canUp || canDown) && Mathf.Approximately(parent.Value, _lastValue))
                {
                    var nextSelectable = canUp ? _up : _down;
                    EventSystem.current.SetSelectedGameObject(nextSelectable.gameObject);
                }
            }
        }
    }
}