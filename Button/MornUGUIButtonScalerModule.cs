using System;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    public sealed class MornUGUIButtonScalerModule : MornUGUIButtonModuleBase
    {
        [SerializeField] private bool _isActive;
        [SerializeField] private float _focusedScale = 1.1f;
        [SerializeField] private float _unfocusedScale = 1.0f;
        [SerializeField] private float _lerpT = 100;
        private float _aimScale;

        public override void Awake(MornUGUIButton parent)
        {
            _aimScale = _unfocusedScale;
        }

        public override void OnDisable(MornUGUIButton parent)
        {
            OnDeselect(parent);
        }

        public override void OnDeselect(MornUGUIButton parent)
        {
            _aimScale = _unfocusedScale;
        }

        public override void OnSelect(MornUGUIButton parent)
        {
            _aimScale = _focusedScale;
        }

        public override void Update(MornUGUIButton parent)
        {
            if (parent.transform == null || !_isActive)
            {
                return;
            }

            var currentScale = parent.transform.localScale.x;
            var newScale = Mathf.Lerp(currentScale, _aimScale, Time.unscaledDeltaTime * _lerpT);
            parent.transform.localScale = Vector3.one * newScale;
        }
    }
}