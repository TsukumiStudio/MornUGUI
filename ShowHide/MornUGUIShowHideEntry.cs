using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal class MornUGUIShowHideEntry
    {
        [SerializeField] private MornUGUIShowHideBase _target;
        [SerializeField] private bool _toShow;

        public async UniTask ExecuteAsync(CancellationToken ct = default)
        {
            if (_toShow)
            {
                await _target.ShowAsync(ct);
            }
            else
            {
                await _target.HideAsync(ct);
            }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(MornUGUIShowHideEntry))]
    public class MornUGUIShowHideSetterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // プロパティを取得
            var targetProp = property.FindPropertyRelative("_target");
            var toShowProp = property.FindPropertyRelative("_toShow");

            // レイアウトの計算
            const float checkBoxWidth = 20f;
            const float spacing = 5f;

            // ラベルを描画
            var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            EditorGUI.LabelField(labelRect, label);

            // ObjectFieldを描画（少し右側を開ける）
            var objectFieldRect = new Rect(
                position.x + EditorGUIUtility.labelWidth + spacing,
                position.y,
                position.width - EditorGUIUtility.labelWidth - checkBoxWidth - spacing * 2,
                position.height);
            EditorGUI.PropertyField(objectFieldRect, targetProp, GUIContent.none);

            // チェックボックスを右端に描画
            var checkBoxRect = new Rect(
                position.x + position.width - checkBoxWidth,
                position.y,
                checkBoxWidth,
                position.height);
            toShowProp.boolValue = EditorGUI.Toggle(checkBoxRect, toShowProp.boolValue);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
#endif
}
