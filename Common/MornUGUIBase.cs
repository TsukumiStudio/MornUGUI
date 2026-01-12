using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    public abstract class MornUGUIBase : Selectable, ISubmitHandler, IPointerClickHandler
    {
        private List<MornUGUIModuleBase> _modules;
        private List<MornUGUIModuleBase> Modules
        {
            get
            {
                if (_modules != null) return _modules;
                _modules = CreateModules();
                foreach (var module in _modules)
                {
                    module.DestroyCancellationToken = destroyCancellationToken;
                }

                return _modules;
            }
        }
        internal abstract List<MornUGUIModuleBase> CreateModules();

        private void Execute(Action<MornUGUIModuleBase> action)
        {
            foreach (var module in Modules)
            {
                action(module);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Execute(module => module.Awake());
        }

        protected virtual void Update()
        {
            Execute(module => module.Update());
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Execute(module => module.OnEnable());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Execute(module => module.OnDisable());
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            Execute(module => module.OnSelect());
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            Execute(module => module.OnDeselect());
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
            Execute(module => module.OnMove(eventData));
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            Execute(module => module.OnPointerEnter(eventData));
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            Execute(module => module.OnPointerExit(eventData));
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            Execute(module => module.OnPointerDown(eventData));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Execute(module => module.OnPointerClick(eventData));
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Execute(module => module.OnSubmit());
        }

        protected virtual void ValueChanged()
        {
            Execute(module => module.OnValueChanged());
        }
    }
}