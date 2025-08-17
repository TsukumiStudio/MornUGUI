using MornEditor;
using TMPro;
using UnityEngine;

namespace MornUGUI
{
    [ExecuteAlways]
    public sealed class MornUGUITextSetter : MonoBehaviour
    {
        [SerializeField, ReadOnly] private TMP_Text _text;
        [SerializeField, HideIf(nameof(HasInheritedSizeSettings))] private MornUGUITextSizeSettings _sizeSettings;
        [ShowIf(nameof(HasInheritedSizeSettings))] public MornUGUITextSizeSettings InheritedSizeSettings;
        [SerializeField, HideIf(nameof(HasInheritedFontSettings))] private MornUGUIFontSettings _fontSettings;
        [ShowIf(nameof(HasInheritedSizeSettings))] public MornUGUIFontSettings InheritedFontSettings;
        [SerializeField, HideIf(nameof(HasInheritedMaterialType))] private MornUGUIMaterialType _materialType;
        [ShowIf(nameof(HasInheritedMaterialType))] public MornUGUIMaterialType InheritedMaterialType;
        private bool HasInheritedSizeSettings => InheritedSizeSettings != null;
        private bool HasInheritedFontSettings => InheritedFontSettings != null;
        private bool HasInheritedMaterialType => InheritedMaterialType != null;
        public TMP_FontAsset Font => _text.font;
        private MornUGUITextSizeSettings SizeSettings => InheritedSizeSettings ?? _sizeSettings;
        private MornUGUIFontSettings FontSettings => InheritedFontSettings ?? _fontSettings;
        private MornUGUIMaterialType MaterialType => InheritedMaterialType ?? _materialType;

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
            _text = GetComponent<TMP_Text>();
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
            if (global == null || _text == null)
            {
                return;
            }

            if (FontSettings != null && MaterialType != null)
            {
                var fontChanged = _text.font != FontSettings.Font;
                var materialChanged = _text.fontSharedMaterial != FontSettings.GetMaterial(MaterialType);
                if (fontChanged || materialChanged)
                {
                    _text.font = FontSettings.Font;
                    _text.fontMaterial = FontSettings.GetMaterial(MaterialType);
                    MornUGUIGlobal.Log("Font/Material Adjusted");
                    MornUGUIGlobal.SetDirty(_text);
                }
            }

            if (SizeSettings != null)
            {
                var autoSizeChanged = _text.enableAutoSizing == false;
                var maxFontSizeChanged = !Mathf.Approximately(_text.fontSizeMax, SizeSettings.FontSize);
                var minFontSizeChanged = !Mathf.Approximately(_text.fontSizeMin, 0);
                var characterSpacingChanged = !Mathf.Approximately(
                    _text.characterSpacing,
                    SizeSettings.CharacterSpacing);
                var lineSpacingChanged = !Mathf.Approximately(_text.lineSpacing, SizeSettings.LineSpacing);
                if (autoSizeChanged
                    || maxFontSizeChanged
                    || minFontSizeChanged
                    || characterSpacingChanged
                    || lineSpacingChanged)
                {
                    _text.enableAutoSizing = true;
                    _text.fontSizeMax = SizeSettings.FontSize;
                    _text.fontSizeMin = 0;
                    _text.characterSpacing = SizeSettings.CharacterSpacing;
                    _text.lineSpacing = SizeSettings.LineSpacing;
                    MornUGUIGlobal.Log("FontSize Adjusted");
                    MornUGUIGlobal.SetDirty(_text);
                }
            }
        }
    }
}