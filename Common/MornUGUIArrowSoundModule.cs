using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIArrowSoundModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _isIgnoreOnCursor;
        [SerializeField] private bool _isIgnoreOnSubmit;
        [SerializeField] private AudioClip _overrideCursorClip;
        [SerializeField] private AudioClip _overrideSubmitClip;
        private IMornUGUIMovable _movable;

        public void Initialize(IMornUGUIMovable movable)
        {
            _movable = movable;
        }

        public override void OnSelect()
        {
            if (_isIgnoreOnCursor) return;
            var clip = _overrideCursorClip ? _overrideCursorClip : MornUGUIGlobal.I.ButtonCursorClip;
            clip.PlayOneShotOnMornUGUI();
        }

        public override void OnMove(AxisEventData axis)
        {
            if (_isIgnoreOnCursor) return;
            if (_movable.IsHorizontal)
            {
                if (axis.moveDir != MoveDirection.Left && axis.moveDir != MoveDirection.Right) return;
                if (axis.moveDir == MoveDirection.Left && !_movable.CanLeft) return;
                if (axis.moveDir == MoveDirection.Right && !_movable.CanRight) return;
            }

            if (_movable.IsVertical)
            {
                if (axis.moveDir != MoveDirection.Up && axis.moveDir != MoveDirection.Down) return;
                if (axis.moveDir == MoveDirection.Up && !_movable.CanUpper) return;
                if (axis.moveDir == MoveDirection.Down && !_movable.CanBottom) return;
            }

            var clip = _overrideCursorClip ? _overrideCursorClip : MornUGUIGlobal.I.ButtonCursorClip;
            clip.PlayOneShotOnMornUGUI();
        }
    }
}