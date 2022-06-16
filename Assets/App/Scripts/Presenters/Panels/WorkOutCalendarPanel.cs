using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RSG;
using Smartplank.Scripts.Configs;
using Smartplank.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Smartplank.Scripts.Presenters.Panels
{
    public class WorkOutCalendarPanel : PanelBase
    {
        [SerializeField]
        private OneDayChallenge workoutPrefab;
        [SerializeField]
        private Transform workoutParent;
        [SerializeField]
        private Button resetChallengeButton;
        [SerializeField]
        private Button undoResetButton;

        private PanelsController _panelsController;
        private Day30ChallengeModel _day30ChallengeModel;
        private Day30ChallengeConfig _day30ChallengeConfig;
        private DataRepository _dataRepository;
        private List<OneDayChallenge> oneDayChallenges = new List<OneDayChallenge>();

        private Day30ChallengeModel day30ChallengeModelBackUp;

        [Inject]
        private void Construct(PanelsController panelsController,
            Day30ChallengeConfig day30ChallengeConfig, DataRepository dataRepository)
        {
            _panelsController = panelsController;
            _day30ChallengeConfig = day30ChallengeConfig;
            _dataRepository = dataRepository;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _day30ChallengeModel = _dataRepository.GetData<Day30ChallengeModel>();
            closeButton.onClick.AddListener(_panelsController.OpenMainMenu);
            resetChallengeButton.onClick.AddListener(ResetChallenges);
            undoResetButton.onClick.AddListener(UndoReset);
            CreateChallengeDays();
        }

        private void ResetChallenges()
        {
            day30ChallengeModelBackUp = new Day30ChallengeModel();
            var backUpArray = new int[_day30ChallengeModel.FinishedIndexes.Count];
            Array.Copy(_day30ChallengeModel.FinishedIndexes.ToArray(), backUpArray, _day30ChallengeModel.FinishedIndexes.Count);
            day30ChallengeModelBackUp.FinishedIndexes = backUpArray.ToList();

            _day30ChallengeModel.FinishedIndexes.Clear();

            _dataRepository.Clear<Day30ChallengeModel>();
            undoResetButton.gameObject.SetActive(true);
            SetUpChallenges(); 
        }

        private void UndoReset()
        {
            var backUpArray = new int[_day30ChallengeModel.FinishedIndexes.Count];
            Array.Copy(day30ChallengeModelBackUp.FinishedIndexes.ToArray(), backUpArray, day30ChallengeModelBackUp.FinishedIndexes.Count);
            _day30ChallengeModel.FinishedIndexes = backUpArray.ToList();

            _dataRepository.Save(_day30ChallengeModel, true);
            undoResetButton.gameObject.SetActive(false);
        }

        private void CreateChallengeDays()
        {
            int i = 0;
            foreach (var item in _day30ChallengeConfig.ChallengeDayItems)
            {
                var newChallengeDay = Instantiate(workoutPrefab, workoutParent);
                newChallengeDay.Init(item, i + 1, _day30ChallengeModel.FinishedIndexes.Contains(i), (i) => GoChallenge(i));
                i++;
                oneDayChallenges.Add(newChallengeDay);
            }
        }

        private void GoChallenge(int i)
        {
            if (_day30ChallengeModel.FinishedIndexes.Contains(i) == false)
            {
                if (_day30ChallengeConfig.ChallengeDayItems[i].ChallengeTimes.Length == 0)
                {
                    _day30ChallengeModel.FinishedIndexes.Add(i);
                    _dataRepository.Save(_day30ChallengeModel, true);
                    SetUpChallenges();
                }

                var promise = new Promise();
                    promise.Done(() =>
                    {
                        _panelsController.CloseLastPanel(false);
                        _panelsController.OpenPanel<WorkOutChallengePanel, WorkOutChallengePanel.SetUpData>(new WorkOutChallengePanel.SetUpData()
                        {
                            times = _day30ChallengeConfig.ChallengeDayItems[i].ChallengeTimes
                        }) ;
                    });
                promise.Catch((_) => {
                    _panelsController.OpenMainMenu();
                });
                var setUpData = new LoadingGamePanel.TimerData()
                {
                    promise = promise,
                    duration = 5
                };
                _panelsController.OpenPanel<LoadingGamePanel, LoadingGamePanel.TimerData>(setUpData);
            }
        }

        public override void OnShowing()
        {
            base.OnShowing();
            SetUpChallenges();
            undoResetButton.gameObject.SetActive(false);
        }

        private void SetUpChallenges()
        {
            int i = 1;
            foreach (var item in oneDayChallenges)
            {
                item.SetUp(_day30ChallengeModel.FinishedIndexes.Contains(i));
            }
        }

        public override void OnHidding()
        {
            base.OnHidding();
            _dataRepository.Save(_day30ChallengeModel, true);
        }
    }
}
