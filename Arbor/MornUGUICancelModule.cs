using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal class MornUGUICancelModule : MornUGUIModuleBase
    {
        [SerializeField] private Selectable _cancelTarget;

        public override void OnStateUpdate(MornUGUIControlState parent)
        {
            if (_cancelTarget == null)
            {
                return;
            }

            if (MornUGUIGlobal.I.InputCancel.WasPerformedThisFrame())
            {
                var current = EventSystem.current.currentSelectedGameObject;
                if (current != _cancelTarget.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(_cancelTarget.gameObject);
                }
                else
                {
                    ExecuteEvents.Execute(
                        _cancelTarget.gameObject,
                        new BaseEventData(EventSystem.current),
                        ExecuteEvents.submitHandler);
                }
            }
        }
    }
}