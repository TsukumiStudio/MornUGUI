using UnityEngine;

namespace MornLib
{
    [CreateAssetMenu(fileName = nameof(MornUGUITextSizeSettings), menuName = "Morn/" + nameof(MornUGUITextSizeSettings))]
    public sealed class MornUGUITextSizeSettings : ScriptableObject
    {
        public int FontSize = 50;
        public int CharacterSpacing;
        public int LineSpacing;
    }
}