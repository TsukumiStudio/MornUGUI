using System;
using UnityEngine;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIButtonColorModule : MornUGUIButtonModuleBase
    {
        [SerializeField] private Image _image;
        [Header("interactable")]
        [SerializeField] private Color _focusedColor = Color.white;
        [SerializeField] private Color _unfocusedColor = Color.gray;
        [Header("not interactable")]
        [SerializeField] private Color _focusedColor2 = Color.white;
        [SerializeField] private Color _unfocusedColor2 = Color.gray;
        private bool _cachedIsFocused;

        public override void Awake(MornUGUIButton parent)
        {
            Update(parent);
        }
        
        public override void OnDisable(MornUGUIButton parent)
        {
            OnDeselect(parent);
        }

        public override void Update(MornUGUIButton parent)
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

        public override void OnSelect(MornUGUIButton parent)
        {
            _cachedIsFocused = true;
            Update(parent);
        }

        public override void OnDeselect(MornUGUIButton parent)
        {
            _cachedIsFocused = false;
            Update(parent);
        }
    }
}