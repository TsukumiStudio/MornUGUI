using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal abstract class MornUGUIShowHideModuleBase
    {
        [SerializeField] protected MornUGUIShowHideTimeSettings Time;
        public abstract void OnAwake(MornUGUIShowHideBase parent);
        public abstract void OnValidate(MornUGUIShowHideBase parent);
        public abstract void OnShowImmediate();
        public abstract void OnHideImmediate();
        public abstract UniTask ShowAsync(CancellationToken ct = default);
        public abstract UniTask HideAsync(CancellationToken ct = default);
    }
}