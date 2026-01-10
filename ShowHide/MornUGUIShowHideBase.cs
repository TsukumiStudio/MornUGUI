using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MornLib
{
    internal abstract class MornUGUIShowHideBase : MonoBehaviour
    {
        public abstract UniTask ShowAsync(CancellationToken ct = default);
        public abstract UniTask HideAsync(CancellationToken ct = default);
        public abstract void DebugShow();
        public abstract void DebugHide();
    }
}