using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace MornLib
{
    [RequireComponent(typeof(Slider))]
    internal sealed class MornUGUISlider : MonoBehaviour,
        ISelectHandler,
        IDeselectHandler,
        ISubmitHandler,
        IMoveHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private MornUGUISliderActiveModule _activeModule;
        [SerializeField] private MornUGUISliderColorModule _colorModule;
        [SerializeField] private MornUGUISliderConvertPointerToSelectModule _convertPointerToSelectModule;
        [SerializeField] private MornUGUISliderNavigationModule _navigationModule;
        [SerializeField] private MornUGUISliderSoundModule _soundModule;
        [Inject] private MornUGUIService _uguiCtrl; 
        public MornUGUIService UGUICtrl => _uguiCtrl;
        public bool IsInteractable { get; set; }
        public Slider.Direction Direction => _slider.direction;
        public float Value => _slider.value;
        public float MinValue => _slider.minValue;
        public float MaxValue => _slider.maxValue;

        private IEnumerable<MornUGUISliderModuleBase> GetModules()
        {
            yield return _activeModule;
            yield return _colorModule;
            yield return _convertPointerToSelectModule;
            yield return _navigationModule;
            yield return _soundModule;
        }

        private void Execute(Action<MornUGUISliderModuleBase, MornUGUISlider> action)
        {
            foreach (var module in GetModules())
            {
                action(module, this);
            }
        }

        private void Awake()
        {
            _slider.OnValueChangedAsObservable()
                   .Subscribe(_ => Execute((module, parent) => module.OnValueChanged(parent)))
                   .AddTo(this);
            Execute((module, parent) => module.Awake(parent));
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

        public void OnMove(AxisEventData eventData)
        {
            // 自分から自分への移動のみ
            if (eventData.selectedObject != gameObject)
            {
                return;
            }

            Execute((module, parent) => module.OnMove(parent, eventData));
        }
    }
}