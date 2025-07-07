using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MornUGUI
{
    /// <summary>拡張可能なScrollRectコンポーネント</summary>
    [RequireComponent(typeof(ScrollRect))]
    public sealed class MornUGUIScrollRect : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private MornUGUIScrollRectAutoScrollModule _autoScrollModule;
        [SerializeField] private MornUGUIScrollRectScrollbarStepModule _scrollbarStepModule;
        public ScrollRect ScrollRect => _scrollRect;
        public bool IsVertical => _scrollRect.vertical;
        public bool IsHorizontal => _scrollRect.horizontal;
        public Scrollbar VerticalScrollbar => _scrollRect.verticalScrollbar;
        public Scrollbar HorizontalScrollbar => _scrollRect.horizontalScrollbar;
        public RectTransform ViewPort => _scrollRect.viewport;
        public RectTransform Content => _scrollRect.content;

        private void Awake()
        {
            if (_scrollRect == null)
            {
                _scrollRect = GetComponent<ScrollRect>();
            }

            Execute((module, scrollRect) => module.Awake(scrollRect));
        }

        private void OnDestroy()
        {
            Execute((module, scrollRect) => module.OnDestroy(scrollRect));
        }

        private void Update()
        {
            Execute((module, scrollRect) => module.OnUpdate(scrollRect));
        }

        private void Reset()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

        private IEnumerable<MornUGUIScrollRectModuleBase> GetModules()
        {
            yield return _scrollbarStepModule;
            yield return _autoScrollModule;
        }

        private void Execute(Action<MornUGUIScrollRectModuleBase, MornUGUIScrollRect> action)
        {
            foreach (var module in GetModules())
            {
                if (module != null)
                {
                    action(module, this);
                }
            }
        }
    }
}