using System;
using System.Collections;
using System.Collections.Generic;
using Smartplank.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Smartplank.Scripts.Presenters.Panels
{
    public class WorkOutChallengePanel : PanelBase<WorkOutChallengePanel.SetUpData>
    {
        public struct SetUpData
        {
            public int[] times;
            public int addIndex;
        }

        [SerializeField] private WorkoutAction[] workoutActions;
        [SerializeField] private WorkoutRest[] workoutRests;
        [SerializeField] private Button resetButton;

        private int currentIndex = 0;

        private PanelsController _panelsController;
        private Day30ChallengeModel _day30ChallengeModel;
        private DataRepository _dataRepository;
        private int pauseTime = 10;
        private bool isPause;

        [Inject]
        private void Construct(PanelsController panelsController,
            DataRepository dataRepository)
        {
            _panelsController = panelsController;
            _dataRepository = dataRepository;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _day30ChallengeModel = _dataRepository.GetData<Day30ChallengeModel>();
            closeButton.onClick.AddListener(() => _panelsController.OpenPanel<WorkOutCalendarPanel>());
            resetButton.onClick.AddListener(ResetLastTimer);
        }

        public override void OnShowing()
        {
            base.OnShowing();
            currentIndex = 0;

            foreach (var item in workoutRests)
            {
                item.SetTime(pauseTime);
            }

            for (int i = 0; i < setUpData.times.Length; i++)
            {
                workoutActions[i].SetTime(setUpData.times[i], false);
            }

            Invoke(nameof(WaitForInput), 0.5f);
        }

        private void WaitForInput()
        {
            workoutActions[currentIndex].SetUp(StartTimer);
        }

        private void StartTimer()
        {
            foreach (var item in workoutActions)
            {
                item.ClearListeners();
            }

            StartCoroutine(nameof(TimerCouroutine));
        }

        private IEnumerator TimerCouroutine()
        {
            float startTime = Time.time;
            float delta = Time.time - startTime;
            float ttl = setUpData.times[currentIndex];
            while (delta < ttl)
            {
                delta = Time.time - startTime;
                workoutActions[currentIndex].SetTime(ttl - delta);
                yield return null;
            }
            if (currentIndex < workoutActions.Length - 1)
            {
                StartCoroutine(PauseCouroutine());
            }
            else
            {
                _day30ChallengeModel.FinishedIndexes.Add(setUpData.addIndex);
                _dataRepository.Save(_day30ChallengeModel, true);
                _panelsController.CloseLastPanel();
            }
        }

        private IEnumerator PauseCouroutine()
        {
            isPause = true;
            float startTime = Time.time;
            float delta = Time.time - startTime;
            float ttl = pauseTime;
            while (delta < ttl)
            {
                delta = Time.time - startTime;
                workoutRests[currentIndex].SetTime(ttl - delta);
                yield return null;
            }
            currentIndex++;
            WaitForInput();
            isPause = false;
        }

        private void ResetLastTimer()
        {
            if (!isPause)
            {
                if (currentIndex >= setUpData.times.Length)
                    return;
                StopAllCoroutines();
                workoutActions[currentIndex].SetTime(setUpData.times[currentIndex], false);
                WaitForInput();
            }
        }
    }
}
