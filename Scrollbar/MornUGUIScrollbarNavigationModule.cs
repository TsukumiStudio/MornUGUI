using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIScrollbarNavigationModule : MornUGUIScrollbarModuleBase
    {
        [SerializeField] private Selectable _up;
        [SerializeField] private Selectable _down;
        [SerializeField] private Selectable _left;
        [SerializeField] private Selectable _right;
        private float _lastValue;

        public override void OnValueChanged(MornUGUIScrollbar parent)
        {
            _lastValue = parent.Value;
        }

        public override void OnMove(MornUGUIScrollbar parent, AxisEventData axisEventData)
        {
            switch (parent.Direction)
            {
                case Scrollbar.Direction.LeftToRight:
                    MoveHorizontal(parent, axisEventData, true);
                    break;
                case Scrollbar.Direction.RightToLeft:
                    MoveHorizontal(parent, axisEventData, false);
                    break;
                case Scrollbar.Direction.BottomToTop:
                    MoveVertical(parent, axisEventData, true);
                    break;
                case Scrollbar.Direction.TopToBottom:
                    MoveVertical(parent, axisEventData, false);
                    break;
            }
        }

        private void MoveHorizontal(MornUGUIScrollbar parent, AxisEventData axisEventData, bool isToRight)
        {
            var toLeft = axisEventData.moveDir == MoveDirection.Left && _left != null;
            var toRight = axisEventData.moveDir == MoveDirection.Right && _right != null;
            if (toLeft || toRight)
            {
                var atMin = Mathf.Approximately(parent.Value, 0);
                var atMax = Mathf.Approximately(parent.Value, 1);
                
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

        private void MoveVertical(MornUGUIScrollbar parent, AxisEventData axisEventData, bool isTop)
        {
            var toUp = axisEventData.moveDir == MoveDirection.Up && _up != null;
            var toDown = axisEventData.moveDir == MoveDirection.Down && _down != null;
            if (toUp || toDown)
            {
                var atMin = Mathf.Approximately(parent.Value, 0);
                var atMax = Mathf.Approximately(parent.Value, 1);
                
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