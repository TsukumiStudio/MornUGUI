using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MornEditor;
using MornUtil;
using UnityEngine;

namespace MornUGUI
{
    [Serializable]
    internal class MornUGUIAnimationModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _active;
        [SerializeField, ShowIf(nameof(Active))] private BindAnimatorClip _inAnimation;
        [SerializeField, ShowIf(nameof(Active))] private BindAnimatorClip _outAnimation;
        private bool _isIn;
        private bool Active => _active;
        private CancellationTokenSource _cancellationTokenSource;
        public bool HasOutAnimation => _active && _outAnimation.IsValid;

        public override void OnStateBegin(MornUGUIControlState parent)
        {
            PlayInAnimationAsync(parent.destroyCancellationToken).Forget();
        }

        public override void OnStateEnd(MornUGUIControlState parent)
        {
            PlayOutAnimationAsync(parent.destroyCancellationToken).Forget();
        }

        public async UniTask PlayInAnimationAsync(CancellationToken cancellationToken = default)
        {
            if (!_active || !_inAnimation.IsValid || _isIn)
            {
                return;
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _isIn = true;
            await PlayAnimation(_inAnimation, _cancellationTokenSource.Token);
        }

        public async UniTask PlayOutAnimationAsync(CancellationToken cancellationToken = default)
        {
            if (!_active || !_outAnimation.IsValid || !_isIn)
            {
                return;
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _isIn = false;
            await PlayAnimation(_outAnimation, _cancellationTokenSource.Token);
        }

        private async UniTask PlayAnimation(BindAnimatorClip bindAnimatorClip, CancellationToken cancellationToken)
        {
            if (!bindAnimatorClip.IsValid)
            {
                return;
            }

            var animator = bindAnimatorClip.Animator;
            var clip = bindAnimatorClip.Clip;
            animator.Play(clip.name, 0, 0f);
            var normalizedTime = 0f;
            while (normalizedTime < 1f)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName(clip.name))
                {
                    normalizedTime = stateInfo.normalizedTime;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }
    }
}