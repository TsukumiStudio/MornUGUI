using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MornEditor;
using UnityEngine;

namespace MornUGUI
{
    public sealed class MornUGUIShowHide : MornUGUIShowHideBase
    {
        [SerializeField] private bool _fadeEnabled;
        [SerializeField, ShowIf(nameof(_fadeEnabled))] private MornUGUIShowHideFadeModule _fadeModule;
        [SerializeField] private bool _moveEnabled;
        [SerializeField, ShowIf(nameof(_moveEnabled))] private MornUGUIShowHideMoveModule _moveModule;

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
            var tasks = new List<UniTask>();
            foreach (var module in GetModules())
            {
                tasks.Add(module.ShowAsync(ct));
            }

            return UniTask.WhenAll(tasks);
        }

        public override UniTask HideAsync(CancellationToken ct = default)
        {
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
            foreach (var module in GetModules())
            {
                module.OnShowImmediate();
            }
        }

        [Button]
        public override void DebugHide()
        {
            foreach (var module in GetModules())
            {
                module.OnHideImmediate();
            }
        }
    }
}