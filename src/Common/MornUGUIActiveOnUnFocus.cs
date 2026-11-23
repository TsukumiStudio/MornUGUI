using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUIActiveOnUnFocus))]
    internal sealed class MornUGUIActiveOnUnFocus : MornUGUIMonoBase
    {
        private MonoBehaviour _owner;
        private bool _isApplyingFocus;

        public override void Initialize(MonoBehaviour owner)
        {
            _owner = owner;
            if (!Application.isPlaying) return;
            SyncActive();
        }

        private void OnEnable()
        {
            // OnDeselect 中は EventSystem がまだ自分の持ち主を選択中と返すので、ここで同期すると表示し直した直後に消えてしまう
            if (!Application.isPlaying || _isApplyingFocus) return;
            SyncActive();
        }

        private void SyncActive()
        {
            var es = EventSystem.current;
            var ownerGo = _owner != null ? _owner.gameObject : null;
            var isSelected = es != null && ownerGo != null && es.currentSelectedGameObject == ownerGo;
            var desired = !isSelected;
            if (gameObject.activeSelf != desired)
            {
                gameObject.SetActive(desired);
            }
        }

        public override void OnSelect()
        {
            if (!Application.isPlaying) return;
            gameObject.SetActive(false);
        }

        public override void OnDeselect()
        {
            if (!Application.isPlaying) return;
            _isApplyingFocus = true;
            gameObject.SetActive(true);
            _isApplyingFocus = false;
        }
    }
}
