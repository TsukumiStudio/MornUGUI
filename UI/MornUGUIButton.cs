using System.Collections.Generic;
using UnityEngine;

namespace MornLib
{
    public sealed class MornUGUIButton : MornUGUIBase, IMornUGUIObject, IMornUGUIInteractable
    {
        [Header("MornUGUIButton")]
        public bool IsLocked;
        public bool IsNegative;
        [Header("Modules")]
        [SerializeField] private MornUGUIActiveModule _activeModule = new();
        [SerializeField] private MornUGUIColorModule _colorModule = new();
        [SerializeField] private MornUGUIPointerModule _pointerModule = new();
        [SerializeField] private MornUGUIScaleModule _scalerModule = new();
        [SerializeField] private MornUGUISoundModule _soundModule = new();
        private List<MornUGUIModuleBase> _module;
        bool IMornUGUIInteractable.IsLocked => IsLocked;
        bool IMornUGUIInteractable.IsNegative => IsNegative;
        Transform IMornUGUIObject.Transform => transform;
        GameObject IMornUGUIObject.GameObject => gameObject;

        internal override List<MornUGUIModuleBase> CreateModules()
        {
            if (_module != null) return _module;
            _module = new List<MornUGUIModuleBase>();
            _activeModule.Initialize();
            _module.Add(_activeModule);
            _colorModule.Initialize(this);
            _module.Add(_colorModule);
            _pointerModule.Initialize(this);
            _module.Add(_pointerModule);
            _scalerModule.Initialize(this);
            _module.Add(_scalerModule);
            _soundModule.Initialize(this);
            _module.Add(_soundModule);
            return _module;
        }
    }
}