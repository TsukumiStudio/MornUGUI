using System;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal class MornUGUISoundBlockModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _ignore;
        [SerializeField] [ReadOnly] private int _leftFrame;

        public override void OnStateBegin(MornUGUIControlState parent)
        {
            if (_ignore)
            {
                return;
            }

            var blockFrame = MornUGUIGlobal.I.BlockFrame;
            if (blockFrame > 0)
            {
                _leftFrame = blockFrame;
                parent.UGUICtrl.BlockOn();
            }
        }

        public override void OnStateUpdate(MornUGUIControlState parent)
        {
            if (_ignore)
            {
                return;
            }

            if (_leftFrame > 0)
            {
                _leftFrame--;
                if (_leftFrame == 0)
                {
                    parent.UGUICtrl.BlockOff();
                }
            }
        }

        public override void OnStateEnd(MornUGUIControlState parent)
        {
            if (_ignore)
            {
                return;
            }

            parent.UGUICtrl.BlockOff();
        }
    }
}