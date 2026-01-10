using System;
using UniRx;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIButtonToggleModule : MornUGUIButtonModuleBase
    {
        [SerializeField] private GameObject _selectedOn;
        [SerializeField] private GameObject _unSelectedOn;
        [SerializeField] private GameObject _selectedOff;
        [SerializeField] private GameObject _unSelectedOff;
        [SerializeField] private bool _isOn;
        private readonly Subject<bool> _toggleSubject = new Subject<bool>();
        private bool _isSelected;
        public bool IsOn
        {
            get => _isOn;
            set
            {
                _isOn = value;
                ApplyIsOn();
                _toggleSubject.OnNext(_isOn);
            }
        }
        public IObservable<bool> OnValueChanged => _toggleSubject;

        public override void Awake(MornUGUIButton parent)
        {
            ApplyIsOn();
        }
        
        public override void OnSubmit(MornUGUIButton parent)
        {
            _isOn = !_isOn;
            ApplyIsOn();
            _toggleSubject.OnNext(_isOn);
        }

        public override void OnSelect(MornUGUIButton parent)
        {
            _isSelected = true;
            ApplyIsOn();
        }

        public override void OnDeselect(MornUGUIButton parent)
        {
            _isSelected = false;
            ApplyIsOn();
        }

        private void ApplyIsOn()
        {
            var selectedOn = _isOn && _isSelected;
            var unSelectedOn = _isOn && !_isSelected;
            var selectedOff = !_isOn && _isSelected;
            var unSelectedOff = !_isOn && !_isSelected;
            if (_selectedOn != null)
            {
                _selectedOn.SetActive(selectedOn);
            }

            if (_unSelectedOn != null)
            {
                _unSelectedOn.SetActive(unSelectedOn);
            }

            if (_selectedOff != null)
            {
                _selectedOff.SetActive(selectedOff);
            }

            if (_unSelectedOff != null)
            {
                _unSelectedOff.SetActive(unSelectedOff);
            }
        }
    }
}