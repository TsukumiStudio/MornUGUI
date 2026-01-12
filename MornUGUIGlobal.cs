using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace MornLib
{
    [CreateAssetMenu(fileName = nameof(MornUGUIGlobal), menuName = "Morn/" + nameof(MornUGUIGlobal))]
    public sealed class MornUGUIGlobal : MornGlobalBase<MornUGUIGlobal>
    {
        protected override string ModuleName => "MornUGUI";
        [Header("Input")]
        [SerializeField] private InputActionReference _submit;
        [SerializeField] private InputActionReference _cancel;
        [Header("Audio")]
        [SerializeField] private AudioMixerGroup _seMixerGroup;
        [SerializeField] private AudioClip _buttonCursorClip;
        [SerializeField] private AudioClip _buttonSubmitClip;
        [SerializeField] private AudioClip _buttonCancelClip;
        [Header("SoundBlock")]
        [SerializeField] private int _soundBlockFrame = 3;
        [Header("Materials")]
        [SerializeField] private string[] _materialNames;
        public InputAction InputSubmit => _submit.action;
        public InputAction InputCancel => _cancel.action;
        public AudioMixerGroup SeMixerGroup => _seMixerGroup;
        public AudioClip ButtonCursorClip => _buttonCursorClip;
        public AudioClip ButtonSubmitClip => _buttonSubmitClip;
        public AudioClip ButtonCancelClip => _buttonCancelClip;
        public int BlockFrame => _soundBlockFrame;
        public string[] MaterialNames => _materialNames;

        internal static void SetDirty(Object obj)
        {
            I.SetDirtyInternal(obj);
        }
    }
}