using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Scrollbar;

namespace MornLib
{
    [RequireComponent(typeof(Scrollbar))]
    internal sealed class MornUGUIScrollbar : MonoBehaviour, IMoveHandler, ISelectHandler, ISubmitHandler
    {
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private MornUGUIScrollbarActiveModule _activeModule;
        [SerializeField] private MornUGUIScrollbarNavigationModule _navigationModule;
        [SerializeField] private MornUGUIScrollbarSoundModule _soundModule;
        public Direction Direction => _scrollbar.direction;
        public float Value => _scrollbar.value;
        public float Size => _scrollbar.size;

        private IEnumerable<MornUGUIScrollbarModuleBase> GetModules()
        {
            yield return _activeModule;
            yield return _navigationModule;
            yield return _soundModule;
        }

        private void Execute(Action<MornUGUIScrollbarModuleBase, MornUGUIScrollbar> action)
        {
            foreach (var module in GetModules())
            {
                action(module, this);
            }
        }

        private void OnEnable()
        {
            Execute((module, parent) => module.OnEnable(parent));
        }

        private void OnDisable()
        {
            Execute((module, parent) => module.OnDisable(parent));
        }

        private void Awake()
        {
            _scrollbar.OnValueChangedAsObservable().Subscribe(_ => Execute((module, parent) => module.OnValueChanged(parent))).AddTo(this);
            Execute((module, parent) => module.Awake(parent));
        }

        public void ToUp()
        {
            SimulateScroll(true);
        }

        public void ToBottom()
        {
            SimulateScroll(false);
        }

        private void SimulateScroll(bool toUp)
        {
            // Find the associated ScrollRect
            var scrollRect = GetComponentInParent<ScrollRect>();
            if (scrollRect == null)
            {
                // If no ScrollRect in parent, check if this scrollbar is referenced by any ScrollRect
                scrollRect = FindObjectsByType<ScrollRect>(FindObjectsSortMode.None)
                   .FirstOrDefault(sr => sr.verticalScrollbar == _scrollbar || sr.horizontalScrollbar == _scrollbar);
            }

            if (scrollRect == null) return;
            var delta = toUp ? scrollRect.scrollSensitivity : -scrollRect.scrollSensitivity;
            delta *= 6;

            // Create a fake scroll event
            var eventData = new PointerEventData(EventSystem.current)
            {
                scrollDelta = Direction == Direction.BottomToTop || Direction == Direction.TopToBottom ? new Vector2(0, delta) : new Vector2(delta, 0)
            };

            // Execute the scroll event on the ScrollRect
            ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.scrollHandler);
        }

        void IMoveHandler.OnMove(AxisEventData eventData)
        {
            Execute((module, parent) => module.OnMove(parent, eventData));
        }

        void ISelectHandler.OnSelect(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnSelect(parent));
        }

        void ISubmitHandler.OnSubmit(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnSubmit(parent));
        }
    }
}