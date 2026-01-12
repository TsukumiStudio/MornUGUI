using System;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Scrollbar;

namespace MornLib
{
    public sealed class MornUGUISelector : MornUGUIBase,
        IMornUGUISelector,
        IMornUGUIInteractable,
        IMornUGUIMovable,
        IMornUGUIArrow
    {
        [Header("MornUGUISelector")]
        [SerializeField] private Direction _direction;
        [SerializeField] private IntReactiveProperty _value;
        [SerializeField] private Vector2Int _valueRange;
        [Header("Modules")]
        [SerializeField] private MornUGUIActiveModule _activeModule;
        [SerializeField] private MornUGUIArrowModule _arrowModule;
        [SerializeField] private MornUGUIColorModule _colorModule;
        [SerializeField] private MornUGUILimitNavigationModule _limitNavigationModule;
        [SerializeField] private MornUGUIArrowSoundModule _soundModule;
#if USE_MORN_LOCALIZE
        [SerializeField] private MornUGUISelectorTextModule _textModule;
#endif
        public int Value
        {
            get => _value.Value;
            set => _value.Value = Mathf.Clamp(value, _valueRange.x, _valueRange.y);
        }
        public IObservable<int> OnValueChanged => _value;

        internal override List<MornUGUIModuleBase> CreateModules()
        {
            var result = new List<MornUGUIModuleBase>();
            _activeModule.Initialize();
            result.Add(_activeModule);
            _arrowModule.Initialize(this, this);
            result.Add(_arrowModule);
            _colorModule.Initialize(this);
            result.Add(_colorModule);
            _limitNavigationModule.Initialize(this);
            result.Add(_limitNavigationModule);
            _soundModule.Initialize(this);
            result.Add(_soundModule);
#if USE_MORN_LOCALIZE
            result.Add(_textModule);
            _textModule.Initialize(this);
#endif
            return result;
        }

        private bool IsAtMin => Value <= _valueRange.x;
        private bool IsAtMax => Value >= _valueRange.y;
        Vector2Int IMornUGUISelector.ValueRange => _valueRange;
        int IMornUGUISelector.Value => Value;
        bool IMornUGUIInteractable.IsLocked => false;
        bool IMornUGUIInteractable.IsNegative => false;
        bool IMornUGUIMovable.IsHorizontal => _direction is Direction.LeftToRight or Direction.RightToLeft;
        bool IMornUGUIMovable.IsVertical => _direction is Direction.BottomToTop or Direction.TopToBottom;
        bool IMornUGUIMovable.CanUpper => _direction == Direction.BottomToTop ? !IsAtMax : !IsAtMin;
        bool IMornUGUIMovable.CanBottom => _direction == Direction.BottomToTop ? !IsAtMin : !IsAtMax;
        bool IMornUGUIMovable.CanLeft => _direction == Direction.LeftToRight ? !IsAtMin : !IsAtMax;
        bool IMornUGUIMovable.CanRight => _direction == Direction.LeftToRight ? !IsAtMax : !IsAtMin;
        Selectable IMornUGUIMovable.UpNavigationTarget => FindSelectableOnUp();
        Selectable IMornUGUIMovable.DownNavigationTarget => FindSelectableOnDown();
        Selectable IMornUGUIMovable.LeftNavigationTarget => FindSelectableOnLeft();
        Selectable IMornUGUIMovable.RightNavigationTarget => FindSelectableOnRight();

        protected override void Awake()
        {
            base.Awake();
            _value.Subscribe(_ => ValueChanged());
        }

        public void OnUpSubmit()
        {
            if (_direction == Direction.BottomToTop && !IsAtMax) Value++;
            else if (_direction == Direction.TopToBottom && !IsAtMin) Value--;
        }

        public void OnBottomSubmit()
        {
            if (_direction == Direction.BottomToTop && !IsAtMin) Value--;
            else if (_direction == Direction.TopToBottom && !IsAtMax) Value++;
        }

        public void OnLeftSubmit()
        {
            if (_direction == Direction.LeftToRight && !IsAtMin) Value--;
            else if (_direction == Direction.RightToLeft && !IsAtMax) Value++;
        }

        public void OnRightSubmit()
        {
            if (_direction == Direction.LeftToRight && !IsAtMax) Value++;
            else if (_direction == Direction.RightToLeft && !IsAtMin) Value--;
        }
    }
}