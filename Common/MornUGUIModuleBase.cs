using UnityEngine.EventSystems;

namespace MornLib
{
    internal abstract class MornUGUIModuleBase
    {
        public virtual void Awake()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {
        }

        public virtual void OnSelect()
        {
        }

        public virtual void OnDeselect()
        {
        }

        public virtual void OnMove(AxisEventData eventData)
        {
        }

        public virtual void OnSubmit()
        {
        }

        public virtual void OnValueChanged()
        {
        }
    }
}