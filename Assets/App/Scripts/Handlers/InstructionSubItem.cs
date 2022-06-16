using System.Collections;
using System.Collections.Generic;
using Smartplank.Localization;
using Smartplank.Scripts.Presenters.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Smartplank.Scripts
{
    public class InstructionSubItem : MonoBehaviour, ISetupable<InstructionSubItem.SetUpData>
    {
        public struct SetUpData
        {
            public string textkey;
            public Sprite iconSprite;
        }

        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text tMP_Text;


        public void SetUp(SetUpData setupdData)
        {
            icon.sprite = setupdData.iconSprite;
            var localisation = tMP_Text.GetComponent<LocalizedText>();
            if(localisation == null)
                localisation = tMP_Text.gameObject.AddComponent<LocalizedText>();

            localisation.LocalizationKey = setupdData.textkey;
            localisation.ForceLocalize();
        }
    }
}
