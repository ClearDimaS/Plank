using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.Configs
{
    [CreateAssetMenu(fileName = "InstructionsConfig", menuName = "Configs/InstructionsConfig")]
    public class InstructionsConfig : ScriptableObject
    {
        [field: SerializeField] public Instruction[] Instructions { get; private set; }

        [System.Serializable]
        public class Instruction
        {
            public string TitleKey;
            public SubItem[] SubItems;

            [System.Serializable]
            public class SubItem
            {
                public Sprite subItemIcon;
                public string subItemKey;
            }
        }
    }
}
