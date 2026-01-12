using UnityEngine;

namespace MornLib
{
    [AddComponentMenu("")]
    public class MornUGUIService : MornGlobalMonoBase<MornUGUIService>
    {
        protected override string ModuleName => "MornUGUIService";
        private AudioSource _seSource;
        public bool IsBlocking { get; private set; }

        protected override void OnInitialized()
        {
            _seSource = gameObject.AddComponent<AudioSource>();
            _seSource.playOnAwake = false;
            _seSource.loop = false;
            _seSource.outputAudioMixerGroup = MornUGUIGlobal.I.SeMixerGroup;
        }

        public void BlockOn()
        {
            IsBlocking = true;
        }

        public void BlockOff()
        {
            IsBlocking = false;
        }

        public void PlayOneShot(AudioClip clip)
        {
            if (_seSource != null && clip != null && Application.isFocused)
            {
                _seSource.MornPlayOneShot(clip);
            }
        }
    }
}