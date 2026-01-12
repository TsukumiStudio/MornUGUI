using UnityEngine;

namespace MornLib
{
    public static class MornUGUIUtil
    {
        public static void AddSoundBlockFactor(string factor)
        {
            MornUGUIService.I.AddSoundBlockFactor(factor);
        }

        public static void RemoveSoundBlockFactor(string factor)
        {
            MornUGUIService.I.RemoveSoundBlockFactor(factor);
        }

        public static void PlayOneShotOnMornUGUI(this AudioClip clip)
        {
            MornUGUIService.I.PlayOneShot(clip);
        }
    }
}