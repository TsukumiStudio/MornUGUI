using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MornLib
{
    [CreateAssetMenu(fileName = nameof(MornUGUIFontSettings), menuName = "Morn/" + nameof(MornUGUIFontSettings))]
    public sealed class MornUGUIFontSettings : ScriptableObject
    {
        [Serializable]
        private class MaterialSet
        {
            public MornUGUIMaterialType MaterialType;
            public Material Material;
        }

        [SerializeField] public TMP_FontAsset Font;
        [SerializeField, Label("廃止予定")] private Material[] Materials;
        [SerializeField] private List<MaterialSet> _materialSets;

        public Material GetMaterial(MornUGUIMaterialType materialType)
        {
            var materialSet = _materialSets.Find(set => set.MaterialType == materialType);
            if (materialSet != null)
            {
                return materialSet.Material;
            }

            if (Materials != null && Materials.Length > 0 && materialType.Index < Materials.Length)
            {
                return Materials[materialType.Index];
            }

            MornUGUIGlobal.Logger.LogError("Materialが見つかりません: " + materialType);
            return null;
        }
    }
}