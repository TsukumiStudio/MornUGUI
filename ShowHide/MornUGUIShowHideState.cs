using System.Collections.Generic;
using Arbor;
using Cysharp.Threading.Tasks;
using MornUtil;
using UnityEngine;

namespace MornUGUI
{
    public sealed class MornUGUIShowHideState : StateBehaviour
    {
        [SerializeField] private List<MornUGUIShowHideEntry> _targets;
        [SerializeField] private StateLink _onComplete;
        [SerializeField] private bool _isExecuteAsIsolated;

        public override async void OnStateBegin()
        {
            var ct = _isExecuteAsIsolated ? MornApp.QuitToken : CancellationTokenOnEnd;
            try
            {
                var tasks = new List<UniTask>();
                foreach (var target in _targets)
                {
                    tasks.Add(target.ExecuteAsync(ct));
                }

                await UniTask.WhenAll(tasks);
                Transition(_onComplete);
            }
            catch (System.OperationCanceledException)
            {
            }
        }
    }
}