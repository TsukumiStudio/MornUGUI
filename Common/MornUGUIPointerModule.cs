using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIPointerModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _immediateSubmit = true;
        private bool _isExist;
        private IMornUGUIObject _parent;

        public void Initialize(IMornUGUIObject parent)
        {
            _parent = parent;
        }

        public override void OnDisable()
        {
            _isExist = false;
        }

        public override void Update()
        {
            if (_isExist && EventSystem.current.currentSelectedGameObject != _parent.GameObject)
            {
                EventSystem.current.SetSelectedGameObject(_parent.GameObject);
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!_immediateSubmit) return;
            if (eventData.button != PointerEventData.InputButton.Left) return;
            ExecuteEvents.Execute(
                _parent.GameObject,
                new BaseEventData(EventSystem.current),
                ExecuteEvents.submitHandler);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_immediateSubmit) return;
            if (eventData.button != PointerEventData.InputButton.Left) return;
            ExecuteEvents.Execute(
                _parent.GameObject,
                new BaseEventData(EventSystem.current),
                ExecuteEvents.submitHandler);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            _isExist = true;
            Update();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _isExist = false;
            if (EventSystem.current.currentSelectedGameObject == _parent.GameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}