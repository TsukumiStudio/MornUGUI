using System;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUISoundModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _ignoreCursor;
        [SerializeField] private bool _ignoreSubmit;
        [SerializeField] private bool _ignoreCancel;
        [SerializeField] private AudioClip _overrideCursorClip;
        [SerializeField] private AudioClip _overrideSubmitClip;
        [SerializeField] private AudioClip _overrideCancelClip;
        private IMornUGUISound _sound;

        public void Initialize(IMornUGUISound sound)
        {
            _sound = sound;
        }

        public override void OnSelect()
        {
            if (_ignoreCursor || MornUGUIService.I.IsBlocking)
            {
                return;
            }

            var clip = _overrideCursorClip ? _overrideCursorClip : MornUGUIGlobal.I.ButtonCursorClip;
            MornUGUIService.I.PlayOneShot(clip);
        }

        public override void OnSubmit()
        {
            if ((_ignoreSubmit && !_sound.IsNegative) || (_ignoreCancel && _sound.IsNegative))
            {
                return;
            }

            AudioClip clip;
            if (_sound.IsNegative)
            {
                clip = _overrideCancelClip ? _overrideCancelClip : MornUGUIGlobal.I.ButtonCancelClip;
            }
            else
            {
                clip = _overrideSubmitClip ? _overrideSubmitClip : MornUGUIGlobal.I.ButtonSubmitClip;
            }

            MornUGUIService.I.PlayOneShot(clip);
        }
    }
}