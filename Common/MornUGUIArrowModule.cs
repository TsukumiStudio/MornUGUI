using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIArrowModule : MornUGUIModuleBase
    {
        [SerializeField] private Selectable _upperArrow;
        [SerializeField] private Selectable _bottomArrow;
        [SerializeField] private Selectable _leftArrow;
        [SerializeField] private Selectable _rightArrow;
        private IMornUGUIMovable _movable;
        private IMornUGUIArrow _arrow;

        public void Initialize(IMornUGUIMovable movable, IMornUGUIArrow arrow)
        {
            _movable = movable;
            _arrow = arrow;
        }

        public override void Awake()
        {
            if (_upperArrow != null) _upperArrow.OnSubmitAsObservable().Subscribe(_ => _arrow.OnUpSubmit());
            if (_bottomArrow != null) _bottomArrow.OnSubmitAsObservable().Subscribe(_ => _arrow.OnBottomSubmit());
            if (_leftArrow != null) _leftArrow.OnSubmitAsObservable().Subscribe(_ => _arrow.OnLeftSubmit());
            if (_rightArrow != null) _rightArrow.OnSubmitAsObservable().Subscribe(_ => _arrow.OnRightSubmit());
            UpdateArrow();
        }

        public override void OnEnable()
        {
            UpdateArrow();
        }

        public override void OnValueChanged()
        {
            Observable.NextFrame().Subscribe(_ => UpdateArrow());
        }

        public override void OnMove(AxisEventData eventData)
        {
            // サウンド再生などを考慮するため、1フレーム遅延させてから処理を行う
            Observable.NextFrame().Subscribe(_ =>
            {
                if (_movable.IsVertical && eventData.moveDir == MoveDirection.Up) _arrow.OnUpSubmit();
                else if (_movable.IsVertical && eventData.moveDir == MoveDirection.Down) _arrow.OnBottomSubmit();
                else if (_movable.IsHorizontal && eventData.moveDir == MoveDirection.Left) _arrow.OnLeftSubmit();
                else if (_movable.IsHorizontal && eventData.moveDir == MoveDirection.Right) _arrow.OnRightSubmit();
            });
        }

        private void UpdateArrow()
        {
            if (_movable.IsVertical)
            {
                if (_upperArrow != null) _upperArrow.gameObject.SetActive(_movable.CanUpper);
                if (_bottomArrow != null) _bottomArrow.gameObject.SetActive(_movable.CanBottom);
                if (_leftArrow != null) _leftArrow.gameObject.SetActive(false);
                if (_rightArrow != null) _rightArrow.gameObject.SetActive(false);
            }
            else if (_movable.IsHorizontal)
            {
                if (_leftArrow != null) _leftArrow.gameObject.SetActive(_movable.CanLeft);
                if (_rightArrow != null) _rightArrow.gameObject.SetActive(_movable.CanRight);
                if (_upperArrow != null) _upperArrow.gameObject.SetActive(false);
                if (_bottomArrow != null) _bottomArrow.gameObject.SetActive(false);
            }
        }
    }
}