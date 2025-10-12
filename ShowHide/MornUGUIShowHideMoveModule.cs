using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MornEase;
using UnityEngine;

namespace MornUGUI
{
    [Serializable]
    public sealed class MornUGUIShowHideMoveModule : MornUGUIShowHideModuleBase
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private Vector2 _showPosition;
        [SerializeField] private Vector2 _hidePosition;
        private CancellationTokenSource _cts;

        public override void OnAwake()
        {
            _target.anchoredPosition = _hidePosition;
        }

        public override void OnShowImmediate()
        {
            _target.anchoredPosition = _showPosition;
        }

        public override void OnHideImmediate()
        {
            _target.anchoredPosition = _hidePosition;
        }

        public override async UniTask ShowAsync(CancellationToken ct = default)
        {
            await MoveAsync(true, _target.anchoredPosition, _showPosition, ct);
        }

        public override async UniTask HideAsync(CancellationToken ct = default)
        {
            await MoveAsync(false, _target.anchoredPosition, _hidePosition, ct);
        }

        private async UniTask MoveAsync(bool toShow, Vector2 startPos, Vector2 endPos, CancellationToken ct = default)
        {
            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var token = _cts.Token;
            var duration = toShow ? Time.ShowDuration : Time.HideDuration;
            var delay = toShow ? Time.ShowDelay : Time.HideDelay;
            var elapsed = 0f;
            if (delay > 0f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            }
            
            var easeType = toShow ? Time.ShowEaseType : Time.HideEaseType;

            while (elapsed < duration)
            {
                ct.ThrowIfCancellationRequested();
                elapsed += UnityEngine.Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                var easedT = t.Ease(easeType);
                _target.anchoredPosition = Vector2.LerpUnclamped(startPos, endPos, easedT);
                await UniTask.Yield(token);
            }

            _target.anchoredPosition = endPos;
        }
    }
}