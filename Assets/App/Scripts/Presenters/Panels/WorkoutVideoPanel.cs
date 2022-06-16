using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RSG;
using Smartplank.Scripts.UserInput;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Zenject;

namespace Smartplank.Scripts.Presenters.Panels
{
    public class WorkoutVideoPanel : PanelBase<WorkoutVideoPanel.SetUpData>
    {
        public struct SetUpData
        {
            public string videoString;
            public int Duration;
        }

        [SerializeField] private TMP_Text timeLeftText;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private Slider slider;

        private PanelsController _panelsController;
        private int lastVideoNumber = 1;
        private const int videosCount = 5;

        [Inject]
        private void Construct(PanelsController panelsController)
        {
            _panelsController = panelsController;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            closeButton.onClick.AddListener(_panelsController.CloseLastPanel);
        }

        public override void SetUp(SetUpData setUpData)
        {
            base.SetUp(setUpData);
            var numberString = Regex.Replace(setUpData.videoString, @"[^\d]", ""); ;
            if (int.TryParse(numberString, out int result))
            {
                lastVideoNumber = result;
            }

            videoPlayer.clip = Resources.Load($"Video/{setUpData.videoString}") as VideoClip;
            videoPlayer.isLooping = true;
        }

        public override void OnShowing()
        {
            base.OnShowing();
            timeLeftText.text = setUpData.Duration.ToString();
            StartTimer();
        }

        private void StartTimer()
        {
            Invoke(nameof(Delay), 1f);
        }

        private void Delay()
        {
            if(gameObject.activeInHierarchy)
                StartCoroutine(nameof(TimerCouroutine));
        }

        private void Update()
        {
            if (!SystemInfo.supportsGyroscope)
            {
                slider.value = 0.5f;
                return;
            }

            slider.value = (GyroInputManager.Instance.InputGyroNormalized + 1f) / 2f;
        }

        private IEnumerator TimerCouroutine()
        {
            float startTime = Time.time;
            float delta = Time.time - startTime;
            float ttl = setUpData.Duration;
            videoPlayer.Play();
            while (delta < ttl)
            {
                delta = Time.time - startTime;
                SetTime(ttl - delta);
                yield return null;
            }

            StartNext();
        }

        private void StartNext()
        {
            int nextVidNumber = (lastVideoNumber + 1) % (videosCount + 1); // numbers go from 1 to 5
            if (nextVidNumber == 0)
                nextVidNumber++;
            var promise = new Promise();

            promise.Done(() =>
            {
                _panelsController.CloseLastPanel();
                _panelsController.OpenPanel<WorkoutVideoPanel, WorkoutVideoPanel.SetUpData>(new WorkoutVideoPanel.SetUpData()
                {
                    Duration = setUpData.Duration,
                    videoString = "video" + nextVidNumber.ToString()
                });
            });
            _panelsController.OpenPanel<LoadingGamePanel, LoadingGamePanel.TimerData>(new LoadingGamePanel.TimerData()
            {
                duration = 30,
                promise = promise
            });

            var clip = videoPlayer.clip;
            videoPlayer.clip = null;
            Resources.UnloadAsset(clip);
        }

        private void SetTime(float v)
        {
            timeLeftText.text = ((int)v).ToString();
        }
    }
}
