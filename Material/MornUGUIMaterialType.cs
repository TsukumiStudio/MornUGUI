using System;
using MornEnum;

namespace MornLib
{
    [Serializable]
    public sealed class MornUGUIMaterialType : MornEnumBase
    {
        protected override string[] Values => MornUGUIGlobal.I.MaterialNames;
    }
}