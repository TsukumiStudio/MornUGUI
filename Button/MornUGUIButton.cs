using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [RequireComponent(typeof(Button))]
    public sealed class MornUGUIButton : MonoBehaviour,
        ISelectHandler,
        IDeselectHandler,
        ISubmitHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerClickHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private bool _isNegative;
        [SerializeField] private bool _allowAsFocusCached = true;
        [SerializeField] private MornUGUIButtonActiveModule _activeModule;
        [SerializeField] private MornUGUIButtonColorModule _colorModule;
        [SerializeField] private MornUGUIButtonConvertPointerToSelectModule _convertPointerToSelectModule;
        [SerializeField] private MornUGUIButtonScalerModule _scalerModule;
        [SerializeField] private MornUGUIButtonSoundModule _soundModule;
        [SerializeField] private MornUGUIButtonToggleModule _toggleModule;
        public bool IsInteractable { get; set; }
        public bool IsNegative => _isNegative;
        public bool AllowAsFocusCached => _allowAsFocusCached;
        public IObservable<Unit>　OnButtonSelected => _button.OnSelectAsObservable().Select(_ => Unit.Default);
        public IObservable<Unit> OnButtonSubmit => _button.OnSubmitAsObservable().Select(_ => Unit.Default);
        public MornUGUIButtonToggleModule AsToggle => _toggleModule;
        public MornUGUIButtonActiveModule AsActive => _activeModule;
        private List<MornUGUIButtonModuleBase> _module;
        private List<MornUGUIButtonModuleBase> Modules => _module ??= new List<MornUGUIButtonModuleBase>
        {
            _activeModule,
            _colorModule,
            _convertPointerToSelectModule,
            _scalerModule,
            _soundModule,
            _toggleModule,
        };

        private void Execute(Action<MornUGUIButtonModuleBase, MornUGUIButton> action)
        {
            foreach (var module in Modules)
            {
                action(module, this);
            }
        }

        private void Awake()
        {
            IsInteractable = _button.interactable;
            Execute((module, parent) => module.Awake(parent));
        }

        private void OnDisable()
        {
            if (EventSystem.current == null)
            {
                return;
            }

            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

            Execute((module, parent) => module.OnDisable(parent));
        }

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void Update()
        {
            Execute((module, parent) => module.Update(parent));
        }

        public void OnSelect(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnSelect(parent));
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnDeselect(parent));
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnSubmit(parent));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerEnter(parent));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerExit(parent));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerDown(eventData, parent));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerClick(eventData, parent));
        }
    }
}