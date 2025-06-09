using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornUGUI
{
    [Serializable]
    internal sealed class MornUGUIButtonConvertPointerToSelectModule : MornUGUIButtonModuleBase
    {
        [SerializeField] [ReadOnly] private bool _isExist;

        public override void OnDisable(MornUGUIButton parent)
        {
            _isExist = false;
        }

        public override void Update(MornUGUIButton parent)
        {
            if (_isExist && EventSystem.current.currentSelectedGameObject != parent.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(parent.gameObject);
            }
        }

        public override void OnPointerDown(MornUGUIButton parent)
        {
            ExecuteEvents.Execute(
                parent.gameObject,
                new BaseEventData(EventSystem.current),
                ExecuteEvents.submitHandler);
        }

        public override void OnPointerEnter(MornUGUIButton parent)
        {
            _isExist = true;
            Update(parent);
        }

        public override void OnPointerExit(MornUGUIButton parent)
        {
            _isExist = false;
            if (EventSystem.current.currentSelectedGameObject == parent.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}