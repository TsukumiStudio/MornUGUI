using System;
using TMPro;
using UnityEngine;

namespace MornLib
{
    [ExecuteAlways]
    [Obsolete("廃止予定")]
    public sealed class MornUGUITextSetterOld : MonoBehaviour
    {
        [SerializeField, ReadOnly] public TMP_Text Text;
        [SerializeField] public MornUGUITextSizeSettings SizeSettings;
        [SerializeField] public MornUGUIFontSettings FontSettings;
        [SerializeField] public MornUGUIMaterialType MaterialType;
        
        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                Adjust();
            }
        }

        [ContextMenu("Rebuild")]
        private void Reset()
        {
            Text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                Adjust();
            }
        }

        public void Adjust()
        {
            var global = MornUGUIGlobal.I;
            if (global == null || SizeSettings == null || FontSettings == null || MaterialType == null || Text == null)
            {
                return;
            }
            
            var fontChanged = Text.font != FontSettings.Font;
            var autoSizeChanged = Text.enableAutoSizing == false;
            var maxFontSizeChanged = !Mathf.Approximately(Text.fontSizeMax, SizeSettings.FontSize);
            var minFontSizeChanged = !Mathf.Approximately(Text.fontSizeMin, 0);
            var characterSpacingChanged = !Mathf.Approximately(Text.characterSpacing, SizeSettings.CharacterSpacing);
            var lineSpacingChanged = !Mathf.Approximately(Text.lineSpacing, SizeSettings.LineSpacing);
            var materialChanged = Text.fontSharedMaterial != FontSettings.GetMaterial(MaterialType);
            var anyChanged = fontChanged
                             || autoSizeChanged
                             || maxFontSizeChanged
                             || minFontSizeChanged
                             || characterSpacingChanged
                             || lineSpacingChanged
                             || materialChanged;
            if (Text != null && anyChanged)
            {
                Text.font = FontSettings.Font;
                Text.enableAutoSizing = true;
                Text.fontSizeMax = SizeSettings.FontSize;
                Text.fontSizeMin = 0;
                Text.characterSpacing = SizeSettings.CharacterSpacing;
                Text.lineSpacing = SizeSettings.LineSpacing;
                Text.fontMaterial = FontSettings.GetMaterial(MaterialType);
                MornUGUIGlobal.Logger.Log("Text Adjusted");
                MornUGUIGlobal.SetDirty(Text);
            }
        }
    }
}