using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MornEditor;
using UnityEngine;

namespace MornUGUI
{
    public sealed class MornUGUIShowHideSequence : MornUGUIShowHideBase
    {
        [SerializeField] private List<MornUGUIShowHideBase> _targets;
        [SerializeField] private float _showInterval;
        [SerializeField] private float _hideInterval;
        [SerializeField] private float _showDelay;
        [SerializeField] private float _hideDelay;
        [SerializeField] private bool _hideReverse;
        private CancellationTokenSource _cts;

        public override async UniTask ShowAsync(CancellationToken ct = default)
        {
            await SequenceAsync(true, ct);
        }

        public override async UniTask HideAsync(CancellationToken ct = default)
        {
            await SequenceAsync(false, ct);
        }

        private async UniTask SequenceAsync(bool toShow, CancellationToken ct = default)
        {
            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            ct = _cts.Token;
            var delay = toShow ? _showDelay : _hideDelay;
            if (delay > 0f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: ct);
            }

            var taskList = new List<UniTask>();
            var targets = !toShow && _hideReverse ? _targets.AsReadOnly().Reverse() : _targets;
            foreach (var target in targets)
            {
                if (toShow)
                {
                    taskList.Add(target.ShowAsync(ct));
                    if (_showInterval > 0f)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(_showInterval), cancellationToken: ct);
                    }
                }
                else
                {
                    taskList.Add(target.HideAsync(ct));
                    if (_hideInterval > 0f)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(_hideInterval), cancellationToken: ct);
                    }
                }
            }

            await UniTask.WhenAll(taskList);
        }

        [Button]
        public override void DebugShow()
        {
            _cts?.Cancel();
            _cts = null;
            foreach (var target in _targets)
            {
                target.DebugShow();
            }
        }

        [Button]
        public override void DebugHide()
        {
            _cts?.Cancel();
            _cts = null;
            foreach (var target in _targets)
            {
                target.DebugHide();
            }
        }
    }
}