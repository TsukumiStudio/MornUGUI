using System;
using UnityEngine;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUISliderColorModule : MornUGUISliderModuleBase
    {
        [SerializeField] private Image _image;
        [SerializeField, ShowIf(nameof(HasImage))] private Color _focusedColor = Color.white;
        [SerializeField, ShowIf(nameof(HasImage))] private Color _unfocusedColor = Color.gray;
        [SerializeField, ShowIf(nameof(HasImage))] private Color _focusedColor2 = Color.white;
        [SerializeField, ShowIf(nameof(HasImage))] private Color _unfocusedColor2 = Color.gray;
        private bool HasImage => _image != null;
        private bool _cachedIsFocused;

        public override void Awake(MornUGUISlider parent)
        {
            Update(parent);
        }

        public override void Update(MornUGUISlider parent)
        {
            if (_image == null)
            {
                return;
            }

            if (_cachedIsFocused)
            {
                _image.color = parent.IsInteractable ? _focusedColor : _focusedColor2;
            }
            else
            {
                _image.color = parent.IsInteractable ? _unfocusedColor : _unfocusedColor2;
            }
        }

        public override void OnSelect(MornUGUISlider parent)
        {
            _cachedIsFocused = true;
            Update(parent);
        }

        public override void OnDeselect(MornUGUISlider parent)
        {
            _cachedIsFocused = false;
            Update(parent);
        }
    }
}