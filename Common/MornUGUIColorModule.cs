using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIColorModule : MornUGUIModuleBase
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        [SerializeField, ShowIf(nameof(HasAny))] private Color _focusedColor = Color.white;
        [SerializeField, ShowIf(nameof(HasAny))] private Color _unfocusedColor = Color.gray;
        [SerializeField, ShowIf(nameof(HasAny))] private Color _focusedColor2 = Color.white;
        [SerializeField, ShowIf(nameof(HasAny))] private Color _unfocusedColor2 = Color.gray;
        private bool _isFocused;
        private IMornUGUIInteractable _parent;
        private bool HasAny => _image != null || _text != null;

        public void Initialize(IMornUGUIInteractable parent)
        {
            _parent = parent;
        }

        public override void Awake()
        {
            Update();
        }

        public override void OnDisable()
        {
            OnDeselect();
        }

        public override void Update()
        {
            Color color;
            if (_isFocused) color = _parent.IsLocked ? _focusedColor2 : _focusedColor;
            else color = _parent.IsLocked ? _unfocusedColor2 : _unfocusedColor;
            if (_image != null) _image.color = color;
            if (_text != null) _text.color = color;
        }

        public override void OnSelect()
        {
            _isFocused = true;
            Update();
        }

        public override void OnDeselect()
        {
            _isFocused = false;
            Update();
        }
    }
}