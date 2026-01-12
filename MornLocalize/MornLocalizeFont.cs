#if USE_MORN_LOCALIZE
using UniRx;
using UnityEngine;

namespace MornLib
{
    [ExecuteAlways]
    [RequireComponent(typeof(MornUGUITextSetter))]
    public sealed class MornLocalizeFont : MonoBehaviour
    {
        [SerializeField, ReadOnly] private MornUGUITextSetter _setter;
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
            _setter = GetComponent<MornUGUITextSetter>();
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

            var fontChanged = _setter.Font != fontSettings.Font;
            var materialChanged = _setter.InheritedMaterialType != _settings.MaterialType;
            var anyChanged = fontChanged || materialChanged;
            if (anyChanged)
            {
                _setter.InheritedFontSettings = fontSettings;
                _setter.InheritedMaterialType.Index = _settings.MaterialType.Index;
                _setter.Adjust();
                MornUGUIGlobal.SetDirty(_setter);
            }
        }
    }
}
#endif