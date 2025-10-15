using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MornEase;
using MornEditor;
using MornUtil;
using UnityEngine;
using UnityEngine.UI;

namespace MornUGUI
{
    [Obsolete("MornUGUIShowHide を使用してください。")]
    public sealed class MornUGUIShowHideFadeOld : MornUGUIShowHideBase
    {
        [SerializeField] private Image _targetImage;
        [SerializeField] private CanvasGroup _targetCanvas;
        [SerializeField] private bool _withRaycast;
        [SerializeField] private MornUGUIShowHideTimeSettings _timeSettings;
        [SerializeField, HideIf(nameof(HasTimer))] private float _showDuration = 0.3f;
        [SerializeField, HideIf(nameof(HasTimer))] private float _showDelay;
        [SerializeField, HideIf(nameof(HasTimer))] private float _hideDuration = 0.3f;
        [SerializeField, HideIf(nameof(HasTimer))] private float _hideDelay;
        [SerializeField, HideIf(nameof(HasTimer))] private MornEaseType _easeType = MornEaseType.EaseOutQuart;
        private bool HasTimer => _timeSettings != null;
        private float ShowDuration => _timeSettings != null ? _timeSettings.ShowDuration : _showDuration;
        private float ShowDelay => _timeSettings != null ? _timeSettings.ShowDelay : _showDelay;
        private float HideDuration => _timeSettings != null ? _timeSettings.HideDuration : _hideDuration;
        private float HideDelay => _timeSettings != null ? _timeSettings.HideDelay : _hideDelay;
        private MornEaseType EaseType => _timeSettings != null ? _timeSettings.ShowEaseType : _easeType;
        private CancellationTokenSource _cts;

        private void Awake()
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

        private void Reset()
        {
            _targetImage = GetComponent<Image>();
            _targetCanvas = GetComponent<CanvasGroup>();
        }

        public override async UniTask ShowAsync(CancellationToken ct = default)
        {
            await FadeAsync(true, ct);
        }

        public override UniTask HideAsync(CancellationToken ct = default)
        {
            return FadeAsync(false, ct);
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

            var duration = toShow ? ShowDuration : HideDuration;
            var delay = toShow ? ShowDelay : HideDelay;
            var elapsed = 0f;
            if (delay > 0f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            }

            var startImageAlpha = _targetImage != null ? _targetImage.GetAlpha() : 0f;
            var startCanvasAlpha = _targetCanvas != null ? _targetCanvas.alpha : 0f;
            var endAlpha = toShow ? 1f : 0f;
            while (elapsed < duration)
            {
                ct.ThrowIfCancellationRequested();
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                var easedT = t.Ease(EaseType);
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

        [Button]
        public override void DebugShow()
        {
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

        [Button]
        public override void DebugHide()
        {
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
    }
}