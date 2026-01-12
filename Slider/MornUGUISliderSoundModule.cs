using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUISliderSoundModule : MornUGUISliderModuleBase
    {
        [SerializeField] private bool _ignoreCursor;
        [SerializeField] private bool _ignoreSubmit;
        [SerializeField] private AudioClip _overrideCursorClip;
        [SerializeField] private AudioClip _overrideSubmitClip;

        public override void OnSelect(MornUGUISlider parent)
        {
            if (_ignoreCursor || MornUGUIService.I.IsBlocking)
            {
                return;
            }

            var clip = _overrideCursorClip ? _overrideCursorClip : MornUGUIGlobal.I.ButtonCursorClip;
            MornUGUIService.I.PlayOneShot(clip);
        }

        public override void OnMove(MornUGUISlider parent, AxisEventData axis)
        {
            if (_ignoreCursor || MornUGUIService.I.IsBlocking)
            {
                return;
            }

            var clip = _overrideCursorClip ? _overrideCursorClip : MornUGUIGlobal.I.ButtonCursorClip;
            MornUGUIService.I.PlayOneShot(clip);
        }

        public override void OnSubmit(MornUGUISlider parent)
        {
            if (_ignoreSubmit)
            {
                return;
            }

            var clip = _overrideSubmitClip ? _overrideSubmitClip : MornUGUIGlobal.I.ButtonSubmitClip;
            MornUGUIService.I.PlayOneShot(clip);
        }
    }
}