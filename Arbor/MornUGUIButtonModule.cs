using System;
using System.Collections.Generic;
using System.Linq;
using Arbor;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal class MornUGUIButtonModule : MornUGUIModuleBase
    {
        [Serializable]
        private class ButtonStateLinkSet
        {
            public Button Button;
            public StateLink StateLink;
        }

        [SerializeField] [ReadOnly] private List<ButtonStateLinkSet> _buttonStateLinkSets;

        public override void OnStateBegin(MornUGUIControlState parent)
        {
            foreach (var buttonStateLinkSet in _buttonStateLinkSets)
            {
                var linkSet = buttonStateLinkSet;
                if (linkSet.StateLink == null || linkSet.StateLink.stateID == 0)
                {
                    continue;
                }

                linkSet.Button.OnSubmitAsObservable().Subscribe(_ => OnButtonPressed(parent, linkSet.StateLink).Forget()).AddTo(parent.CancellationTokenOnEnd);
            }
        }

        private async UniTaskVoid OnButtonPressed(MornUGUIControlState parent, StateLink stateLink)
        {
            var animationModule = parent.AnimationModule;
            if (animationModule != null && animationModule.HasOutAnimation)
            {
                await animationModule.PlayOutAnimationAsync(parent.destroyCancellationToken);
            }

            parent.Transition(stateLink);
        }

        public override void OnEditorInitialize(MornUGUIControlState parent)
        {
            var buttons = parent.CanvasGroup.transform.GetComponentsInChildren<Button>().ToList();
            foreach (var button in buttons)
            {
                var index = _buttonStateLinkSets.FindIndex(x => x.Button == button);
                if (index != -1)
                {
                    _buttonStateLinkSets[index].StateLink.name = button.name;
                }
                else
                {
                    _buttonStateLinkSets.Add(new ButtonStateLinkSet { Button = button, StateLink = new StateLink { name = button.name, }, });
                }
            }

            _buttonStateLinkSets.RemoveAll(x => buttons.All(y => y != x.Button));
        }
    }
}