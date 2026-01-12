using System;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIActiveModule : MornUGUIModuleBase
    {
        [SerializeField] private GameObject _focused;
        [SerializeField] private GameObject _unfocused;

        public void Initialize()
        {
        }

        public override void Awake()
        {
            if (_focused != null) _focused.SetActive(false);
            if (_unfocused != null) _unfocused.SetActive(true);
        }

        public override void OnDisable()
        {
            OnDeselect();
        }

        public override void OnDeselect()
        {
            if (_focused != null) _focused.SetActive(false);
            if (_unfocused != null) _unfocused.SetActive(true);
        }

        public override void OnSelect()
        {
            if (_focused != null) _focused.SetActive(true);
            if (_unfocused != null) _unfocused.SetActive(false);
        }
    }
}