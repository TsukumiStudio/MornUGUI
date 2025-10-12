using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MornEase;
using MornUtil;
using UnityEngine;
using UnityEngine.UI;

namespace MornUGUI
{
    [Serializable]
    public sealed class MornUGUIShowHideFadeModule : MornUGUIShowHideModuleBase
    {
        [SerializeField] private Image _targetImage;
        [SerializeField] private CanvasGroup _targetCanvas;
        [SerializeField] private bool _withRaycast;
        private CancellationTokenSource _cts;

        public override void OnAwake()
        {
            if (_targetImage != null)
            {
                _targetImage.SetAlpha(0f);
            }

            if (_targetCanvas != null)
            {
                _targetCanvas.alpha = 0f;
                if (_withRaycast)
                {
                    _targetCanvas.interactable = false;
                    _targetCanvas.blocksRaycasts = false;
                }
            }
        }

        public override void OnShowImmediate()
        {
            if (_targetImage != null)
            {
                _targetImage.SetAlpha(1f);
            }

            if (_targetCanvas != null)
            {
                _targetCanvas.alpha = 1f;
                if (_withRaycast)
                {
                    _targetCanvas.interactable = true;
                    _targetCanvas.blocksRaycasts = true;
                }
            }
        }

        public override void OnHideImmediate()
        {
            if (_targetImage != null)
            {
                _targetImage.SetAlpha(0f);
            }

            if (_targetCanvas != null)
            {
                _targetCanvas.alpha = 0f;
                if (_withRaycast)
                {
                    _targetCanvas.interactable = false;
                    _targetCanvas.blocksRaycasts = false;
                }
            }
        }

        public override async UniTask ShowAsync(CancellationToken ct = default)
        {
            await FadeAsync(true, ct);
        }

        public override async UniTask HideAsync(CancellationToken ct = default)
        {
            await FadeAsync(false, ct);
        }

        private async UniTask FadeAsync(bool toShow, CancellationToken ct = default)
        {
            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var token = _cts.Token;
            if (_targetCanvas != null && _withRaycast)
            {
                _targetCanvas.interactable = toShow;
                _targetCanvas.blocksRaycasts = toShow;
            }

            var duration = toShow ? Time.ShowDuration : Time.HideDuration;
            var delay = toShow ? Time.ShowDelay : Time.HideDelay;
            var elapsed = 0f;
            if (delay > 0f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            }

            var easeType = toShow ? Time.ShowEaseType : Time.HideEaseType;
            var startImageAlpha = _targetImage != null ? _targetImage.GetAlpha() : 0f;
            var startCanvasAlpha = _targetCanvas != null ? _targetCanvas.alpha : 0f;
            var endAlpha = toShow ? 1f : 0f;
            while (elapsed < duration)
            {
                ct.ThrowIfCancellationRequested();
                elapsed += UnityEngine.Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                var easedT = t.Ease(easeType);
                if (_targetImage != null)
                {
                    _targetImage.SetAlpha(Mathf.LerpUnclamped(startImageAlpha, endAlpha, easedT));
                }

                if (_targetCanvas != null)
                {
                    _targetCanvas.alpha = Mathf.LerpUnclamped(startCanvasAlpha, endAlpha, easedT);
                }

                await UniTask.Yield(token);
            }

            if (_targetImage != null)
            {
                _targetImage.SetAlpha(endAlpha);
            }

            if (_targetCanvas != null)
            {
                _targetCanvas.alpha = endAlpha;
            }
        }
    }
}