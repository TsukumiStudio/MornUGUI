using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
            var token = _cts.Token;
            var delay = toShow ? _showDelay : _hideDelay;
            if (delay > 0f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            }
            
            var taskList = new List<UniTask>();
            foreach (var target in _targets)
            {
                if (toShow)
                {
                    taskList.Add(target.ShowAsync(token));
                    if (_showInterval > 0f)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(_showInterval), cancellationToken: token);
                    }
                }
                else
                {
                    taskList.Add(target.HideAsync(token));
                    if (_hideInterval > 0f)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(_hideInterval), cancellationToken: token);
                    }
                }
            }

            await UniTask.WhenAll(taskList);
        }
    }
}