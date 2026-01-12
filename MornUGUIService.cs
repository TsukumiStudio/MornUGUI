using System.Collections.Generic;
using UnityEngine;

namespace MornLib
{
    [AddComponentMenu("")]
    internal class MornUGUIService : MornGlobalMonoBase<MornUGUIService>
    {
        protected override string ModuleName => "MornUGUIService";
        private AudioSource _seSource;
        private readonly List<string> _soundBlockFactors = new();

        protected override void OnInitialized()
        {
            _seSource = gameObject.AddComponent<AudioSource>();
            _seSource.playOnAwake = false;
            _seSource.loop = false;
            _seSource.outputAudioMixerGroup = MornUGUIGlobal.I.SeMixerGroup;
        }

        public void AddSoundBlockFactor(string factor)
        {
            _soundBlockFactors.Add(factor);
        }

        public void RemoveSoundBlockFactor(string factor)
        {
            _soundBlockFactors.Remove(factor);
        }

        public void PlayOneShot(AudioClip clip)
        {
            if (_seSource != null && clip != null && Application.isFocused && _soundBlockFactors.Count == 0)
            {
                _seSource.MornPlayOneShot(clip);
            }
        }
    }
}