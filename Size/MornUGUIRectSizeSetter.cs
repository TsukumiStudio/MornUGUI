using UnityEngine;

namespace MornLib
{
    [RequireComponent(typeof(RectTransform))]
    [ExecuteAlways]
    internal sealed class MornUGUIRectSizeSetter : MonoBehaviour
    {
        [SerializeField] private MornUGUIRectSizeSettings _settings;
        [SerializeField] private RectTransform _rect;

        private void Awake()
        {
            if (Application.isPlaying)
            {
                Adjust();
            }
        }

        private void Reset()
        {
            _rect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                Adjust();
            }
        }

        public void SetSettings(MornUGUIRectSizeSettings settings)
        {
            _settings = settings;
            Adjust();
        }

        private void Adjust()
        {
            var global = MornUGUIGlobal.I;
            if (global == null || _settings == null)
            {
                return;
            }

            if (_rect != null && _rect.sizeDelta != _settings.Size)
            {
                _rect.sizeDelta = _settings.Size;
                MornUGUIGlobal.Logger.Log("Rect Transform Size Adjusted");
                MornUGUIGlobal.SetDirty(_rect);
            }
        }
    }
}