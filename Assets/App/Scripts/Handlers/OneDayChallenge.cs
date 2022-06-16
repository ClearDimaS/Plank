using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Smartplank.Localization;
using Zenject;
using System;

namespace Smartplank.Scripts
{
    public class OneDayChallenge : MonoBehaviour
    {
        [SerializeField] private TMP_Text dayNumberText;
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image background;
        [SerializeField] private GameObject finishedGO;
        [SerializeField] private Button goPlayChallengeBtn;

        [SerializeField] private Color evenColor;
        [SerializeField] private Color oddColor;

        [SerializeField] private Transform actionsParent;
        [SerializeField] private GameObject actionTextPrefab;
        [SerializeField] private GameObject restTextPrefab;
        [SerializeField] private GameObject longRestTextPrefab;

        public void Init(ChallengeDayItem challengeDayItem, int dayNumber, bool isFinished, Action<int> onClick)
        {
            goPlayChallengeBtn.onClick.AddListener(() => onClick?.Invoke(dayNumber - 1));
            background.color = dayNumber % 2 == 1 ? evenColor : oddColor;
            dayNumberText.text = dayNumber + ".";
            SetUp(isFinished);
            CreateFilling(challengeDayItem);
        }

        public void SetUp(bool isFinished)
        {
            toggle.isOn = isFinished;
            finishedGO.gameObject.SetActive(isFinished);
        }

        private void CreateFilling(ChallengeDayItem challengeDayItem)
        {
            if (challengeDayItem.ChallengeTimes.Length == 0)
                Instantiate(longRestTextPrefab, actionsParent);

            for (int i = 0; i < challengeDayItem.ChallengeTimes.Length; i++)
            {
                var time = challengeDayItem.ChallengeTimes[i];
                Instantiate(actionTextPrefab, actionsParent).GetComponentInChildren<TMP_Text>().text = $"{time} ";
                if (i < challengeDayItem.ChallengeTimes.Length)
                    Instantiate(restTextPrefab, actionsParent);
            }
        }
    }
}
