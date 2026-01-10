using System;
using UnityEngine.EventSystems;

namespace MornLib
{
    [Serializable]
    internal abstract class MornUGUIScrollbarModuleBase
    {
        public virtual void Awake(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnEnable(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnDisable(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnValueChanged(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnMove(MornUGUIScrollbar parent, AxisEventData axisEventData)
        {
        }
        
        public virtual void OnSelect(MornUGUIScrollbar parent)
        {
        }
        
        public virtual void OnSubmit(MornUGUIScrollbar parent)
        {
        }
    }
}