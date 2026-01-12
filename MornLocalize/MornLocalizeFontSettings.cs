#if USE_MORN_LOCALIZE
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MornLib
{
    [CreateAssetMenu(fileName = nameof(MornLocalizeFontSettings), menuName = "Morn/" + nameof(MornLocalizeFontSettings))]
    public sealed class MornLocalizeFontSettings : ScriptableObject
    {
        [Serializable]
        private class FontSet
        {
            public string LanguageKey;
            public MornUGUIFontSettings FontSettings;
        }

        [SerializeField] private List<FontSet> _fontSets;
        public MornUGUIMaterialType MaterialType;

        public MornUGUIFontSettings GetFontSettings(string languageKey)
        {
            foreach (var fontSet in _fontSets)
            {
                if (fontSet.LanguageKey == languageKey)
                {
                    return fontSet.FontSettings;
                }
            }

            MornUGUIGlobal.Logger.LogError("FontSettingsが見つかりません: " + languageKey);
            return null;
        }
    }
}
#endif