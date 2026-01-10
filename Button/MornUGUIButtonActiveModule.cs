using System;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    public sealed class MornUGUIButtonActiveModule : MornUGUIButtonModuleBase
    {
        [SerializeField] private GameObject _focused;
        [SerializeField] private GameObject _unfocused;
        public GameObject Focused => _focused;
        public GameObject Unfocused => _unfocused;

        public override void Awake(MornUGUIButton parent)
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

        public override void OnDisable(MornUGUIButton parent)
        {
            OnDeselect(parent);
        }

        public override void OnDeselect(MornUGUIButton parent)
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

        public override void OnSelect(MornUGUIButton parent)
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