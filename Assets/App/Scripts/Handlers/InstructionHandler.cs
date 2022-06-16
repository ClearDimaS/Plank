using System.Collections;
using System.Collections.Generic;
using Smartplank.Localization;
using Smartplank.Scripts.Configs;
using Smartplank.Scripts.Presenters.Panels;
using TMPro;
using UnityEngine;

namespace Smartplank.Scripts
{
    public class InstructionHandler : MonoBehaviour, ISetupable<InstructionHandler.SetUpData>
    {
        public struct SetUpData
        {
            public InstructionsConfig.Instruction instructionData;
        }

        [SerializeField] private InstructionSubItem instructionSubItem;
        [SerializeField] private Transform subItemParent;
        [SerializeField] private TMP_Text titleText;

        public void SetUp(SetUpData setupdData)
        {
            foreach (var subItem in setupdData.instructionData.SubItems)
            {
                var subItemComponent = Instantiate(instructionSubItem, subItemParent);
                subItemComponent.SetUp(new InstructionSubItem.SetUpData()
                {
                    iconSprite = subItem.subItemIcon,
                    textkey = subItem.subItemKey
                });
                var localisation = titleText.GetComponent<LocalizedText>();
                if (localisation == null)
                    localisation = titleText.gameObject.AddComponent<LocalizedText>();

                localisation.LocalizationKey = setupdData.instructionData.TitleKey;
                localisation.ForceLocalize();
            }
        }
    }
}
