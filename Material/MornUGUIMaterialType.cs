using System;
using MornEnum;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIMaterialType : MornEnumBase
    {
        protected override string[] Values => MornUGUIGlobal.I.MaterialNames;
    }
}