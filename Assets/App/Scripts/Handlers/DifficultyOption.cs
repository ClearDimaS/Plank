using System;
using System.Collections;
using System.Collections.Generic;
using Smartplank.Localization;
using Smartplank.Scripts.Presenters.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Smartplank.Scripts.WorkoutCourse.TimeOption;

namespace Smartplank.Scripts
{
    public class DifficultyOption : MonoBehaviour, IInitable<DifficultyOption.OptionData>
    {
        public struct OptionData
        {
            public string difficultyKey;
            public Action<DifficultyOption> onClick;
        }

        [SerializeField] private TMP_Text difficultyText;
        [SerializeField] private TMP_Text difficultyText1;
        [SerializeField] private Button chooseButton;

        [SerializeField] private GameObject unChosenGO;
        [SerializeField] private GameObject chosenGO;

        public void Init(OptionData initData)
        {
            SetLocalizedText(difficultyText, initData.difficultyKey);
            SetLocalizedText(difficultyText1, initData.difficultyKey);

            chooseButton.onClick.AddListener(() => initData.onClick?.Invoke(this));
        }

        public void Select()
        {
            unChosenGO.gameObject.SetActive(false);
            chosenGO.gameObject.SetActive(true);
        }

        public void DeSelect()
        {
            unChosenGO.gameObject.SetActive(true);
            chosenGO.gameObject.SetActive(false);
        }

        private void SetLocalizedText(TMP_Text TMPtext, string text)
        {
            LocalizedText localaized = TMPtext.GetComponent<LocalizedText>();
            if(localaized == null)
                localaized = TMPtext.gameObject.AddComponent<LocalizedText>();

            localaized.LocalizationKey = text;
            localaized.ForceLocalize();
        }
    }
}
