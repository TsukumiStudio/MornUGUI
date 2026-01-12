#if USE_MORN_LOCALIZE
using System;
using UnityEngine;

namespace MornLib
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [Obsolete("廃止予定")]
    public sealed class MornLocalizeButtonOld : MonoBehaviour
    {
        [SerializeField, ReadOnly] private MornLocalizeText[] _texts;
        [SerializeField, ReadOnly] private MornLocalizeFontOld[] _fonts;
        [SerializeField, ReadOnly] private MornUGUITextSetterOld[] _textSetter;
        [SerializeField] private MornLocalizeString _text;
        [SerializeField] private MornLocalizeFontSettings _fontSettings;
        [SerializeField] private MornUGUITextSizeSettings _sizeSettings;

        [ContextMenu("Rebuild")]
        private void Reset()
        {
            _texts = GetComponentsInChildren<MornLocalizeText>();
            _fonts = GetComponentsInChildren<MornLocalizeFontOld>();
            _textSetter = GetComponentsInChildren<MornUGUITextSetterOld>();
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                Adjust();
            }
        }

        private void Adjust()
        {
            var global = MornLocalizeGlobal.I;
            if (global == null)
            {
                return;
            }

            if (_text != null)
            {
                foreach (var text in _texts)
                {
                    if (!text.Text.IsEqual(_text))
                    {
                        text.Text = _text;
                        text.Adjust(global.DebugLanguageKey);
                        MornUGUIGlobal.SetDirty(text);
                    }
                }
            }

            if (_fontSettings != null)
            {
                foreach (var font in _fonts)
                {
                    if (font.Settings != _fontSettings)
                    {
                        font.Settings = _fontSettings;
                        font.Adjust(global.DebugLanguageKey);
                        MornUGUIGlobal.SetDirty(font);
                    }
                }
            }

            if (_sizeSettings != null)
            {
                foreach (var setter in _textSetter)
                {
                    if (setter.SizeSettings != _sizeSettings)
                    {
                        setter.SizeSettings = _sizeSettings;
                        setter.Adjust();
                        MornUGUIGlobal.SetDirty(setter);
                    }
                }
            }
        }
    }
}
#endif