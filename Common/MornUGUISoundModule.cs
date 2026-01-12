using System;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUISoundModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _isIgnoreOnCursor;
        [SerializeField] private bool _isIgnoreOnSubmit;
        [SerializeField] private bool _isIgnoreOnCancel;
        [SerializeField] private AudioClip _overrideCursorClip;
        [SerializeField] private AudioClip _overrideSubmitClip;
        [SerializeField] private AudioClip _overrideCancelClip;
        private IMornUGUIInteractable _interactable;

        public void Initialize(IMornUGUIInteractable sound)
        {
            _interactable = sound;
        }

        public override void OnSelect()
        {
            if (_isIgnoreOnCursor) return;
            var clip = _overrideCursorClip ? _overrideCursorClip : MornUGUIGlobal.I.ButtonCursorClip;
            clip.PlayOneShotOnMornUGUI();
        }

        public override void OnSubmit()
        {
            AudioClip clip;
            if (_interactable.IsNegative || _interactable.IsLocked)
            {
                if (_isIgnoreOnCancel) return;
                clip = _overrideCancelClip ? _overrideCancelClip : MornUGUIGlobal.I.ButtonCancelClip;
            }
            else
            {
                if (_isIgnoreOnSubmit) return;
                clip = _overrideSubmitClip ? _overrideSubmitClip : MornUGUIGlobal.I.ButtonSubmitClip;
            }

            clip.PlayOneShotOnMornUGUI();
        }
    }
}