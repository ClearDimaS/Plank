using System;
using System.Collections;
using System.Collections.Generic;
using RSG;
using Smartplank.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Smartplank.Scripts.Presenters.Panels
{
    public class WorkOutCoursePanel : PanelBase
    {
        [SerializeField] private Button soundButton;
        [SerializeField] private Button startButton;
        [SerializeField] private Sprite on, off;

        private PanelsController _panelsController;
        private UserSettingsModel _userSettingsModel;
        private DataRepository _dataRepository;
        private WorkoutsModel _workoutsModel;

        [Inject]
        private void Construct(PanelsController panelsController, DataRepository dataRepository)
        {
            _panelsController = panelsController;
            _dataRepository = dataRepository;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _workoutsModel = _dataRepository.GetData<WorkoutsModel>();
            _userSettingsModel = _dataRepository.GetData<UserSettingsModel>();
            closeButton.onClick.AddListener(_panelsController.OpenMainMenu);
            soundButton.onClick.AddListener(SwitchSound);
            startButton.onClick.AddListener(StartWorkout);
            SetSound();
        }

        private void StartWorkout()
        {
            var promise = new Promise();
            promise.Done(() =>
            {
                _panelsController.CloseLastPanel();
                _panelsController.OpenPanel<WorkoutVideoPanel, WorkoutVideoPanel.SetUpData>(new WorkoutVideoPanel.SetUpData()
                {
                    Duration = _workoutsModel.ChosenTime,
                    videoString  = "video1"
                });
            });
            _panelsController.OpenPanel<LoadingGamePanel, LoadingGamePanel.TimerData>(new LoadingGamePanel.TimerData()
            {
                duration = 5,
                promise = promise
            }) ;
        }

        private void SwitchSound()
        {
            _userSettingsModel.isSoundOn = !_userSettingsModel.isSoundOn;
            SetSound();
        }

        private void SetSound()
        {
            soundButton.image.sprite = _userSettingsModel.isSoundOn ? on : off;
        }
    }
}
