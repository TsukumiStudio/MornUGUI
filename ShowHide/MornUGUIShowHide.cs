using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MornLib
{
    internal sealed class MornUGUIShowHide : MornUGUIShowHideBase
    {
        [SerializeField] private bool _fadeEnabled;
        [SerializeField, ShowIf(nameof(_fadeEnabled))] private MornUGUIShowHideFadeModule _fadeModule;
        [SerializeField] private bool _moveEnabled;
        [SerializeField, ShowIf(nameof(_moveEnabled))] private MornUGUIShowHideMoveModule _moveModule;
        private CancellationTokenSource _cts;

        private IEnumerable<MornUGUIShowHideModuleBase> GetModules()
        {
            if (_fadeEnabled && _fadeModule != null)
            {
                yield return _fadeModule;
            }

            if (_moveEnabled && _moveModule != null)
            {
                yield return _moveModule;
            }
        }

        private void Awake()
        {
            foreach (var module in GetModules())
            {
                module.OnAwake(this);
            }
        }

        private void OnValidate()
        {
            foreach (var module in GetModules())
            {
                module.OnValidate(this);
            }
        }

        public override UniTask ShowAsync(CancellationToken ct = default)
        {
            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            ct = _cts.Token;
            var tasks = new List<UniTask>();
            foreach (var module in GetModules())
            {
                tasks.Add(module.ShowAsync(ct));
            }

            return UniTask.WhenAll(tasks);
        }

        public override UniTask HideAsync(CancellationToken ct = default)
        {
            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            ct = _cts.Token;
            var tasks = new List<UniTask>();
            foreach (var module in GetModules())
            {
                tasks.Add(module.HideAsync(ct));
            }

            return UniTask.WhenAll(tasks);
        }

        [Button]
        public override void DebugShow()
        {
            _cts?.Cancel();
            _cts = null;
            foreach (var module in GetModules())
            {
                module.OnShowImmediate();
            }
        }

        [Button]
        public override void DebugHide()
        {
            _cts?.Cancel();
            _cts = null;
            foreach (var module in GetModules())
            {
                module.OnHideImmediate();
            }
        }
    }
}