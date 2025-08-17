using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornUGUI
{
    [Serializable]
    internal sealed class MornUGUISliderConvertPointerToSelectModule : MornUGUISliderModuleBase
    {
        [SerializeField] [ReadOnly] private bool _isExist;

        public override void Update(MornUGUISlider parent)
        {
            if (_isExist && EventSystem.current.currentSelectedGameObject != parent.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(parent.gameObject);
            }
        }

        public override void OnPointerDown(PointerEventData eventData, MornUGUISlider parent)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            
            ExecuteEvents.Execute(
                parent.gameObject,
                new BaseEventData(EventSystem.current),
                ExecuteEvents.submitHandler);
        }

        public override void OnPointerEnter(MornUGUISlider parent)
        {
            _isExist = true;
            Update(parent);
        }

        public override void OnPointerExit(MornUGUISlider parent)
        {
            _isExist = false;
            if (EventSystem.current.currentSelectedGameObject == parent.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}