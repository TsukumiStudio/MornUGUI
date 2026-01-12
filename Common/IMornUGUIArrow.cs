using System.Threading;

namespace MornLib
{
    internal interface IMornUGUIArrow
    {
        void OnUpSubmit();
        void OnBottomSubmit();
        void OnLeftSubmit();
        void OnRightSubmit();
    }
}