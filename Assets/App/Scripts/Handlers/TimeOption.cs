using System;
using System.Collections;
using System.Collections.Generic;
using Smartplank.Scripts.Presenters.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Smartplank.Scripts.WorkoutCourse
{
    public class TimeOption : MonoBehaviour, IInitable<TimeOption.TimeOptionData>
    {
        public struct TimeOptionData
        {
            public int Time;
            public Action<int> onClick;
        }

        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text timeText1;
        [SerializeField] private Button chooseButton;
        [SerializeField] private Image chooseStatusImage;
        [SerializeField] private Color chosenColor;
        [SerializeField] private Color unChosenColor;

        [SerializeField] private GameObject unChosenGO;
        [SerializeField] private GameObject chosenGO;

        public void Init(TimeOptionData initData)
        {
            timeText.text = initData.Time.ToString();
            timeText1.text = initData.Time.ToString();
            chooseButton.onClick.AddListener(() => initData.onClick?.Invoke(initData.Time));
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
    }
}
