using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUILimitNavigationModule : MornUGUIModuleBase
    {
        [SerializeField] private Selectable _up;
        [SerializeField] private Selectable _down;
        [SerializeField] private Selectable _left;
        [SerializeField] private Selectable _right;
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
                    if (_left != null && _parent.IsHorizontal && !_parent.CanLeft) EventSystem.current.SetSelectedGameObject(_left.gameObject);
                    break;
                case MoveDirection.Up:
                    if (_up != null && _parent.IsVertical && !_parent.CanUpper) EventSystem.current.SetSelectedGameObject(_up.gameObject);
                    break;
                case MoveDirection.Right:
                    if (_right != null && _parent.IsHorizontal && !_parent.CanRight) EventSystem.current.SetSelectedGameObject(_right.gameObject);
                    break;
                case MoveDirection.Down:
                    if (_down != null && _parent.IsVertical && !_parent.CanBottom) EventSystem.current.SetSelectedGameObject(_down.gameObject);
                    break;
            }
        }
    }
}