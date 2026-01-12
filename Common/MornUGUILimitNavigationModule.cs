using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUILimitNavigationModule : MornUGUIModuleBase
    {
        private IMornUGUIMovable _parent;

        public void Initialize(IMornUGUIMovable parent)
        {
            _parent = parent;
        }

        public override void OnMove(AxisEventData axisEventData)
        {
            switch (axisEventData.moveDir)
            {
                case MoveDirection.Left:
                    var left = _parent.LeftNavigationTarget;
                    if (left != null && _parent.IsHorizontal && !_parent.CanLeft)
                        EventSystem.current.SetSelectedGameObject(left.gameObject);
                    break;
                case MoveDirection.Up:
                    var up = _parent.UpNavigationTarget;
                    if (up != null && _parent.IsVertical && !_parent.CanUpper)
                        EventSystem.current.SetSelectedGameObject(up.gameObject);
                    break;
                case MoveDirection.Right:
                    var right = _parent.RightNavigationTarget;
                    if (right != null && _parent.IsHorizontal && !_parent.CanRight)
                        EventSystem.current.SetSelectedGameObject(right.gameObject);
                    break;
                case MoveDirection.Down:
                    var down = _parent.DownNavigationTarget;
                    if (down != null && _parent.IsVertical && !_parent.CanBottom)
                        EventSystem.current.SetSelectedGameObject(down.gameObject);
                    break;
            }
        }
    }
}