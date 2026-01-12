using System;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIScaleModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _isActive;
        [SerializeField, ShowIf(nameof(IsActive))] private float _focusedScale = 1.1f;
        [SerializeField, ShowIf(nameof(IsActive))] private float _unfocusedScale = 1.0f;
        [SerializeField, ShowIf(nameof(IsActive))] private float _lerpT = 100;
        private float _aimScale;
        private IMornUGUIObject _parent;
        private bool IsActive => _isActive;

        public void Initialize(IMornUGUIObject parent)
        {
            _parent = parent;
        }

        public override void Awake()
        {
            _aimScale = _unfocusedScale;
        }

        public override void OnDisable()
        {
            OnDeselect();
        }

        public override void OnDeselect()
        {
            _aimScale = _unfocusedScale;
        }

        public override void OnSelect()
        {
            _aimScale = _focusedScale;
        }

        public override void Update()
        {
            if (_parent.Transform == null || !_isActive) return;
            var currentScale = _parent.Transform.localScale.x;
            var newScale = Mathf.Lerp(currentScale, _aimScale, Time.unscaledDeltaTime * _lerpT);
            _parent.Transform.localScale = Vector3.one * newScale;
        }
    }
}