#if UNITY_EDITOR
using MornEnum;
using UnityEditor;
using UnityEngine;

namespace MornLib
{
    [CustomPropertyDrawer(typeof(MornUGUIMaterialType))]
    internal class MornUGUIMaterialTypeDrawer : MornEnumDrawerBase
    {
        protected override string[] Values => MornUGUIGlobal.I.MaterialNames;
        protected override Object PingTarget => MornUGUIGlobal.I;
    }
}
#endif