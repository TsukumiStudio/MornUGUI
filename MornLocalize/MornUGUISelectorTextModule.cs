#if USE_MORN_LOCALIZE
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal class MornUGUISelectorTextModule : MornUGUIModuleBase
    {
        [SerializeField] private List<MornLocalizeString> _texts = new();
        [SerializeField] private TMP_Text _text;
        private IMornUGUISelector _parent;

        public void Initialize(IMornUGUISelector parent)
        {
            _parent = parent;
        }

        public override void OnEnable()
        {
            OnValueChanged();
        }

        public override void OnValueChanged()
        {
            if (_text == null || _texts.Count == 0)
            {
                return;
            }

            var range = _parent.ValueRange;
            var index = Mathf.Clamp(_parent.Value - range.x, 0, _texts.Count - 1);
            _text.text = _texts[index].Get();
        }
    }
}
#endif