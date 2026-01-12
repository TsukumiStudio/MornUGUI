using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    public abstract class MornUGUIBase : Selectable, ISubmitHandler
    {
        private List<MornUGUIModuleBase> _modules;
        private List<MornUGUIModuleBase> Modules => _modules ??= CreateModules();
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
            Execute((module) => module.Update());
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Execute((module) => module.OnEnable());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Execute((module) => module.OnDisable());
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            Execute((module) => module.OnSelect());
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            Execute((module) => module.OnDeselect());
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
            Execute((module) => module.OnMove(eventData));
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Execute((module) => module.OnSubmit());
        }

        protected virtual void ValueChanged()
        {
            Execute((module) => module.OnValueChanged());
        }
    }
}