using UnityEngine;

namespace MornLib
{
    [CreateAssetMenu(fileName = nameof(MornUGUIShowHideTimeSettings), menuName = "Morn/" + nameof(MornUGUIShowHideTimeSettings))]
    internal sealed class MornUGUIShowHideTimeSettings : ScriptableObject
    {
        [SerializeField] private float _showDuration = 0.3f;
        [SerializeField] private float _showDelay;
        [SerializeField] private float _hideDuration = 0.3f;
        [SerializeField] private float _hideDelay;
        [SerializeField] private MornEaseType _showEaseType = MornEaseType.EaseOutQuart;
        [SerializeField] private MornEaseType _hideEaseType = MornEaseType.EaseOutQuart;
        public float ShowDuration => _showDuration;
        public float ShowDelay => _showDelay;
        public float HideDuration => _hideDuration;
        public float HideDelay => _hideDelay;
        public MornEaseType ShowEaseType => _showEaseType;
        public MornEaseType HideEaseType => _hideEaseType;
    }
}