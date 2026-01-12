using System;
using UnityEngine;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIColorModule : MornUGUIModuleBase
    {
        [SerializeField] private Image _image;
        [SerializeField, ShowIf(nameof(HasImage)), Label("選択中")] private Color _focusedColor = Color.white;
        [SerializeField, ShowIf(nameof(HasImage)), Label("非選択中")] private Color _unfocusedColor = Color.gray;
        [SerializeField, ShowIf(nameof(HasImage)), Label("決定不可_選択中")] private Color _focusedColor2 = Color.white;
        [SerializeField, ShowIf(nameof(HasImage)), Label("決定不可_非選択中")] private Color _unfocusedColor2 = Color.gray;
        private bool _isFocused;
        private IMornUGUIInteractable _parent;
        private bool HasImage => _image != null;

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
            if (_image == null) return;
            if (_isFocused) _image.color = _parent.IsActive ? _focusedColor : _focusedColor2;
            else _image.color = _parent.IsActive ? _unfocusedColor : _unfocusedColor2;
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