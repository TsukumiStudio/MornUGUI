using System;
using UnityEngine;

namespace MornUGUI
{
    /// <summary>グリッドレイアウトに基づいてScrollbarのステップ数を自動設定するモジュール</summary>
    [Serializable]
    internal sealed class MornUGUIScrollRectScrollbarStepModule : MornUGUIScrollRectModuleBase
    {
        [SerializeField] private bool _isActive;
        [SerializeField] private int _columnsPerPage = 3;
        [SerializeField] private int _rowsPerPage = 2;
        private int _lastChildCount;

        public override void Awake(MornUGUIScrollRect parent)
        {
            if (!_isActive)
            {
                return;
            }

            UpdateScrollbarSteps(parent);
        }

        public override void OnUpdate(MornUGUIScrollRect parent)
        {
            if (!_isActive)
            {
                return;
            }

            var currentChildCount = GetActiveChildCount(parent);
            if (currentChildCount != _lastChildCount)
            {
                _lastChildCount = currentChildCount;
                UpdateScrollbarSteps(parent);
            }
        }

        private int GetActiveChildCount(MornUGUIScrollRect parent)
        {
            var content = parent.Content;
            var count = 0;
            for (var i = 0; i < content.childCount; i++)
            {
                var child = content.GetChild(i);
                if (child.gameObject.activeInHierarchy)
                {
                    count++;
                }
            }

            return count;
        }

        private void UpdateScrollbarSteps(MornUGUIScrollRect parent)
        {
            var totalItems = GetActiveChildCount(parent);
            if (totalItems == 0)
            {
                return;
            }

            // 横スクロールバーの更新
            if (parent.IsHorizontal && parent.HorizontalScrollbar != null)
            {
                parent.HorizontalScrollbar.numberOfSteps = CalculateHorizontalSteps(totalItems);
            }

            // 縦スクロールバーの更新
            if (parent.IsVertical && parent.VerticalScrollbar != null)
            {
                parent.VerticalScrollbar.numberOfSteps = CalculateVerticalSteps(totalItems);
            }
        }

        private int CalculateHorizontalSteps(int totalItems)
        {
            // 横スクロールの場合、列単位でスクロールする
            // 総列数を計算
            var totalColumns = Mathf.CeilToInt((float)totalItems / _rowsPerPage);
            if (totalColumns <= _columnsPerPage)
            {
                return 0; // スクロール不要
            }

            // スクロール可能な列数（総列数 - 表示列数 + 1）
            var steps = totalColumns - _columnsPerPage + 1;
            return Mathf.Max(2, steps);
        }

        private int CalculateVerticalSteps(int totalItems)
        {
            // 縦スクロールの場合、行単位でスクロールする
            // 総行数を計算
            var totalRows = Mathf.CeilToInt((float)totalItems / _columnsPerPage);
            if (totalRows <= _rowsPerPage)
            {
                return 0; // スクロール不要
            }

            // スクロール可能な行数（総行数 - 表示行数 + 1）
            var steps = totalRows - _rowsPerPage + 1;
            return Mathf.Max(2, steps);
        }
    }
}