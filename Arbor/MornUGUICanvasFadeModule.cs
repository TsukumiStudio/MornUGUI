using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal class MornUGUICanvasFadeModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _active;
        [SerializeField] private float _fadeInDuration = 0.3f;
        [SerializeField] private float _fadeOutDuration = 0.6f;
        private CancellationTokenSource _cancellationTokenSource;

        public override void OnAwake(MornUGUIControlState parent)
        {
            if (!_active)
            {
                return;
            }

            parent.CanvasGroup.alpha = 0;
        }

        public override void OnStateBegin(MornUGUIControlState parent)
        {
            if (!_active)
            {
                return;
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(parent.destroyCancellationToken);
            parent.CanvasGroup.alpha = 0;
            FadeCanvas(parent.CanvasGroup, 1, _fadeInDuration, _cancellationTokenSource.Token).Forget();
        }

        public override void OnStateEnd(MornUGUIControlState parent)
        {
            if (!_active)
            {
                return;
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(parent.destroyCancellationToken);
            FadeCanvas(parent.CanvasGroup, 0, _fadeOutDuration, _cancellationTokenSource.Token).Forget();
        }

        private async static UniTaskVoid FadeCanvas(CanvasGroup target, float to, float duration,
            CancellationToken token)
        {
            var from = target.alpha;
            var startTime = Time.time;
            while (duration > 0)
            {
                var dif = Time.time - startTime;
                var alpha = Mathf.Lerp(from, to, dif / duration);
                target.alpha = alpha;
                if (Mathf.Approximately(target.alpha, to))
                {
                    break;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            token.ThrowIfCancellationRequested();
            target.alpha = to;
        }
    }
}