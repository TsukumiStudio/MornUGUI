#if UNITY_EDITOR
using MornEnum;
using UnityEditor;
using UnityEngine;

namespace MornUGUI
{
    [CustomPropertyDrawer(typeof(MornUGUIMaterialType))]
    public class MornUGUIMaterialTypeDrawer : MornEnumDrawerBase
    {
        protected override string[] Values => MornUGUIGlobal.I.MaterialNames;
        protected override Object PingTarget => MornUGUIGlobal.I;
    }
}
#endif