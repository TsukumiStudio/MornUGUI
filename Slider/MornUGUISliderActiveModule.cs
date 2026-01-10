using System;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUISliderActiveModule : MornUGUISliderModuleBase
    {
        [SerializeField] private GameObject _focused;
        [SerializeField] private GameObject _unfocused;

        public override void Awake(MornUGUISlider parent)
        {
            if (_focused != null)
            {
                _focused?.SetActive(false);
            }

            if (_unfocused != null)
            {
                _unfocused?.SetActive(true);
            }
        }

        public override void OnDeselect(MornUGUISlider parent)
        {
            if (_focused != null)
            {
                _focused?.SetActive(false);
            }

            if (_unfocused != null)
            {
                _unfocused?.SetActive(true);
            }
        }

        public override void OnSelect(MornUGUISlider parent)
        {
            if (_focused != null)
            {
                _focused?.SetActive(true);
            }

            if (_unfocused != null)
            {
                _unfocused?.SetActive(false);
            }
        }
    }
}