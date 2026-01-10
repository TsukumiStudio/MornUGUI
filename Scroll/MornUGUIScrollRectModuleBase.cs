using System;

namespace MornLib
{
    [Serializable]
    internal abstract class MornUGUIScrollRectModuleBase
    {
        public virtual void Awake(MornUGUIScrollRect parent)
        {
        }
        
        public virtual void OnDestroy(MornUGUIScrollRect parent)
        {
        }
        
        public virtual void OnUpdate(MornUGUIScrollRect parent)
        {
        }
    }
}