using UnityEngine.UI;

namespace MornLib
{
    internal interface IMornUGUIMovable
    {
        bool IsVertical { get; }
        bool IsHorizontal { get; }
        bool CanUpper { get; }
        bool CanBottom { get; }
        bool CanLeft { get; }
        bool CanRight { get; }
        Selectable UpNavigationTarget { get; }
        Selectable DownNavigationTarget { get; }
        Selectable LeftNavigationTarget { get; }
        Selectable RightNavigationTarget { get; }
    }
}