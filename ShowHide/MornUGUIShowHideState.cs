using System.Collections.Generic;
using Arbor;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MornUGUI
{
    public sealed class MornUGUIShowHideState : StateBehaviour
    {
        [SerializeField] private List<MornUGUIShowHideEntry> _targets;
        [SerializeField] private StateLink _onComplete;

        public override async void OnStateBegin()
        {
            try
            {
                var tasks = new List<UniTask>();
                foreach (var target in _targets)
                {
                    tasks.Add(target.ExecuteAsync(CancellationTokenOnEnd));
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