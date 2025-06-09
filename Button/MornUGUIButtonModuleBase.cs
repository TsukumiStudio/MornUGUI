using System;

namespace MornUGUI
{
    [Serializable]
    public abstract class MornUGUIButtonModuleBase
    {
        public virtual void Awake(MornUGUIButton parent)
        {
        }
        
        public virtual void OnDisable(MornUGUIButton parent)
        {
        }
        
        public virtual void Update(MornUGUIButton parent)
        {
        }

        public virtual void OnSelect(MornUGUIButton parent)
        {
        }

        public virtual void OnDeselect(MornUGUIButton parent)
        {
        }

        public virtual void OnSubmit(MornUGUIButton parent)
        {
        }

        public virtual void OnPointerEnter(MornUGUIButton parent)
        {
        }

        public virtual void OnPointerExit(MornUGUIButton parent)
        {
        }

        public virtual void OnPointerDown(MornUGUIButton parent)
        {
        }
    }
}