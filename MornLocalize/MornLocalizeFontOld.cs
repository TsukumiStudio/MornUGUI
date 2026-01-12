#if USE_MORN_LOCALIZE
using System;
using UniRx;
using UnityEngine;

namespace MornLib
{
    [ExecuteAlways]
    [RequireComponent(typeof(MornUGUITextSetterOld))]
    [Obsolete("廃止予定")]
    public sealed class MornLocalizeFontOld : MonoBehaviour
    {
        [SerializeField, ReadOnly] private MornUGUITextSetterOld _setter;
        [SerializeField] private MornLocalizeFontSettings _settings;
        public MornLocalizeFontSettings Settings
        {
            get => _settings;
            set => _settings = value;
        }

        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                MornLocalizeCore.OnLanguageChanged.Subscribe(Adjust).AddTo(this);
                Adjust(MornLocalizeCore.CurrentLanguage);
            }
        }

        private void Reset()
        {
            _setter = GetComponent<MornUGUITextSetterOld>();
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                var global = MornLocalizeGlobal.I;
                if (global == null)
                {
                    return;
                }

                Adjust(global.DebugLanguageKey);
            }
        }

        public void Adjust(string languageKey)
        {
            var global = MornLocalizeGlobal.I;
            if (global == null || _setter == null || _settings == null)
            {
                return;
            }

            var fontSettings = _settings.GetFontSettings(languageKey);
            if (fontSettings == null)
            {
                return;
            }

            var fontChanged = _setter.Text.font != fontSettings.Font;
            var materialChanged = _setter.MaterialType.Index != _settings.MaterialType.Index;
            var anyChanged = fontChanged || materialChanged;
            if (anyChanged)
            {
                _setter.FontSettings = fontSettings;
                _setter.MaterialType.Index = _settings.MaterialType.Index;
                _setter.Adjust();
                MornUGUIGlobal.SetDirty(_setter);
            }
        }
    }
}
#endif