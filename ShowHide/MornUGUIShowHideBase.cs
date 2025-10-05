using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MornUGUI
{
    public abstract class MornUGUIShowHideBase : MonoBehaviour
    {
        public abstract UniTask ShowAsync(CancellationToken ct = default);
        public abstract UniTask HideAsync(CancellationToken ct = default);
    }
}