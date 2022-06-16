using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LocalizationTexts", menuName = "Configs/Localization", order = 0)]
    public class LocalizationTexts : ScriptableObject
    {
        [SerializeField] private TextAsset[] textAssets;
        public TextAsset[] TextAssets => textAssets;
    }
}

