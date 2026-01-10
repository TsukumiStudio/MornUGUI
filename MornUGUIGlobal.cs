using UnityEngine;
using UnityEngine.InputSystem;

namespace MornLib
{
    [CreateAssetMenu(fileName = nameof(MornUGUIGlobal), menuName = "Morn/" + nameof(MornUGUIGlobal))]
    public sealed class MornUGUIGlobal : MornGlobalBase<MornUGUIGlobal>
    {
        public override string ModuleName => "MornUGUI";
        [Header("Input")]
        [SerializeField] private InputActionReference _submit;
        [SerializeField] private InputActionReference _cancel;
        [Header("Audio")]
        [SerializeField] private AudioClip _buttonCursorClip;
        [SerializeField] private AudioClip _buttonSubmitClip;
        [SerializeField] private AudioClip _buttonCancelClip;
        [Header("SoundBlock")]
        [SerializeField] private int _soundBlockFrame = 3;
        [Header("Materials")]
        [SerializeField] private string[] _materialNames;
        public InputAction InputSubmit => _submit.action;
        public InputAction InputCancel => _cancel.action;
        public AudioClip ButtonCursorClip => _buttonCursorClip;
        public AudioClip ButtonSubmitClip => _buttonSubmitClip;
        public AudioClip ButtonCancelClip => _buttonCancelClip;
        public int BlockFrame => _soundBlockFrame;
        public string[] MaterialNames => _materialNames;

        internal static void Log(string message)
        {
            I.LogInternal(message);
        }

        internal static void LogWarning(string message)
        {
            I.LogWarningInternal(message);
        }

        internal static void LogError(string message)
        {
            I.LogErrorInternal(message);
        }

        internal static void SetDirty(Object obj)
        {
            I.SetDirtyInternal(obj);
        }
    }
}