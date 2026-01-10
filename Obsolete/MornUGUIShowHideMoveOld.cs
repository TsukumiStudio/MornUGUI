using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MornEase;
using MornEditor;
using UnityEngine;

namespace MornLib
{
    [Obsolete("MornUGUIShowHide を使用してください。")]
    internal sealed class MornUGUIShowHideMoveOld : MornUGUIShowHideBase
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private Vector2 _showPosition;
        [SerializeField] private Vector2 _hidePosition;
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
            _target.anchoredPosition = _hidePosition;
        }

        private void Reset()
        {
            _target = GetComponent<RectTransform>();
        }

        public override async UniTask ShowAsync(CancellationToken ct = default)
        {
            await MoveAsync(true, _target.anchoredPosition, _showPosition, ct);
        }

        public override UniTask HideAsync(CancellationToken ct = default)
        {
            return MoveAsync(false, _target.anchoredPosition, _hidePosition, ct);
        }

        private async UniTask MoveAsync(bool toShow, Vector2 startPos, Vector2 endPos, CancellationToken ct = default)
        {
            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var token = _cts.Token;
            var duration = toShow ? ShowDuration : HideDuration;
            var delay = toShow ? ShowDelay : HideDelay;
            var elapsed = 0f;
            if (delay > 0f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            }

            while (elapsed < duration)
            {
                ct.ThrowIfCancellationRequested();
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                var easedT = t.Ease(EaseType);
                _target.anchoredPosition = Vector2.LerpUnclamped(startPos, endPos, easedT);
                await UniTask.Yield(token);
            }

            _target.anchoredPosition = endPos;
        }

        [Button]
        public override void DebugShow()
        {
            _target.anchoredPosition = _showPosition;
        }

        [Button]
        public override void DebugHide()
        {
            _target.anchoredPosition = _hidePosition;
        }
    }
}