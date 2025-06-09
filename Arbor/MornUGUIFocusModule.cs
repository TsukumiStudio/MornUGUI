using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MornUGUI
{
    [Serializable]
    internal class MornUGUIFocusModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _ignored;
        [SerializeField] private bool _useCache = true;
        [SerializeField] private bool _findAdjacent = true;
        [SerializeField] private Selectable _autoFocusTarget;
        [SerializeField] [ReadOnly] private Selectable _focusCache;
        private PlayerInput _cachedInput;
        private bool _isPointing;
        private Vector2 _cachedPointingPos;

        public override void OnStateBegin(MornUGUIControlState parent)
        {
            if (_autoFocusTarget == null || _ignored)
            {
                return;
            }

            var all = PlayerInput.all;
            if (all.Count == 0)
            {
                MornUGUIGlobal.LogWarning("PlayerInput is not found.");
                _cachedInput = null;
                return;
            }

            if (all.Count > 1)
            {
                MornUGUIGlobal.LogWarning("Multiple PlayerInput is found.");
                _cachedInput = null;
                return;
            }

            _cachedInput = all[0];
            if (_autoFocusTarget != null
                && EventSystem.current.currentSelectedGameObject == _autoFocusTarget.gameObject)
            {
                return;
            }

            // 初回の自動フォーカス
            AutoFocus();
        }

        private void AutoFocus()
        {
            if (_useCache && _focusCache != null && _focusCache.gameObject.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(_focusCache.gameObject);
                MornUGUIGlobal.Log("Focus on cache.");
            }
            else if (_autoFocusTarget != null && _autoFocusTarget.gameObject.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(_autoFocusTarget.gameObject);
                MornUGUIGlobal.Log("Focus on target.");
            }
        }

        public override void OnStateUpdate(MornUGUIControlState parent)
        {
            if (_autoFocusTarget == null || _cachedInput == null || _ignored)
            {
                return;
            }

            // Navigate入力があった際にキャッシュを選択
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (_cachedInput.actions["Navigate"].controls.Any(x => x.IsPressed()))
                {
                    AutoFocus();
                    _isPointing = false;
                }
            }

            if (_cachedInput.actions["Point"].WasPerformedThisFrame())
            {
                var newPoint = _cachedInput.actions["Point"].ReadValue<Vector2>();
                if (_isPointing)
                {
                    _cachedPointingPos = newPoint;
                }
                else
                {
                    if (Vector2.Distance(_cachedPointingPos, newPoint) > 0.1f)
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        _isPointing = true;
                        _cachedPointingPos = newPoint;
                    }
                }
            }

            if (!_useCache)
            {
                return;
            }

            // キャッシュの更新処理
            var currentSelected = EventSystem.current.currentSelectedGameObject;
            var current = currentSelected == null ? null : currentSelected.GetComponent<Selectable>();
            if (current != null && IsFocusable(current))
            {
                _focusCache = current;
            }

            // キャッシュが非アクティブな場合、隣接を探す
            if (_findAdjacent && _focusCache != null && !_focusCache.gameObject.activeInHierarchy)
            {
                var selectable = _focusCache.GetComponent<Selectable>();
                if (selectable != null)
                {
                    var list = new List<Selectable>()
                    {
                        selectable.FindSelectableOnUp(),
                        selectable.FindSelectableOnDown(),
                        selectable.FindSelectableOnLeft(),
                        selectable.FindSelectableOnRight()
                    };
                    var mostNearDistance = float.MaxValue;
                    Selectable mostNear = null;
                    foreach (var near in list)
                    {
                        if (near != null && near.gameObject.activeInHierarchy)
                        {
                            var distance = Vector3.Distance(near.transform.position, _focusCache.transform.position);
                            if (distance < mostNearDistance && IsFocusable(near))
                            {
                                mostNearDistance = distance;
                                mostNear = near;
                            }
                        }
                    }

                    if (mostNear != null)
                    {
                        _focusCache = mostNear;
                        EventSystem.current.SetSelectedGameObject(_focusCache.gameObject);
                        MornUGUIGlobal.Log("Focus on cache near.");
                    }
                }
            }
        }

        private bool IsFocusable(Selectable selectable)
        {
            if (selectable.TryGetComponent<MornUGUIButton>(out var button))
            {
                return button.AllowAsFocusCached;
            }

            return true;
        }

        private async UniTaskVoid DelayAsync(Action action, CancellationToken cancellationToken)
        {
            await UniTask.Yield(cancellationToken);
            action();
        }

        public override void OnStateEnd(MornUGUIControlState parent)
        {
            if (_autoFocusTarget == null || _ignored)
            {
                return;
            }

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}