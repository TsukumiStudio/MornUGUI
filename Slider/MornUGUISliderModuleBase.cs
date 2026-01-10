using System;
using UnityEngine.EventSystems;

namespace MornLib
{
    [Serializable]
    internal abstract class MornUGUISliderModuleBase
    {
        public virtual void OnValueChanged(MornUGUISlider parent)
        {
            
        }
        
        public virtual void Awake(MornUGUISlider parent)
        {
        }

        public virtual void Update(MornUGUISlider parent)
        {
        }

        public virtual void OnSelect(MornUGUISlider parent)
        {
        }

        public virtual void OnDeselect(MornUGUISlider parent)
        {
        }

        public virtual void OnSubmit(MornUGUISlider parent)
        {
        }

        public virtual void OnPointerEnter(MornUGUISlider parent)
        {
        }

        public virtual void OnPointerExit(MornUGUISlider parent)
        {
        }

        public virtual void OnPointerDown(PointerEventData eventData, MornUGUISlider parent)
        {
        }

        public virtual void OnMove(MornUGUISlider parent, AxisEventData axisEventData)
        {
        }
    }
}