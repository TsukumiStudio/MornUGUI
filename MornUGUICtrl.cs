using System;
using MornSound;
using UnityEngine;

namespace MornUGUI
{
    [Serializable]
    public class MornUGUICtrl
    {
        [SerializeField] private AudioSource _seSource;
        public bool IsBlocking { get; private set; }

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