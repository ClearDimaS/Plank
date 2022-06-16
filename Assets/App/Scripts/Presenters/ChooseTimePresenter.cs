using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Smartplank.Scripts.Configs;
using Smartplank.Scripts.Models;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts.WorkoutCourse
{
    public class ChooseTimePresenter : MonoBehaviour
    {
        [SerializeField] private TimeOption timeOptionPrefab;
        [SerializeField] private Transform timeOptionsParent;

        private List<TimeOption> timeOptions = new List<TimeOption>();

        private WorkoutsConfig _workoutsConfig;
        private WorkoutsModel _workoutsModel;
        private DataRepository _dataRepository;

        private int[] _times => _workoutsConfig.WorkOutsTimes;

        [Inject]
        private void Construct(WorkoutsConfig workoutsConfig, DataRepository dataRepository)
        {
            _workoutsConfig = workoutsConfig;
            _dataRepository = dataRepository;
        }

        private void Start()
        {
            _workoutsModel = _dataRepository.GetData<WorkoutsModel>();
            foreach (var time in _times)
            {
                var option = Instantiate(timeOptionPrefab, timeOptionsParent);
                option.Init(new TimeOption.TimeOptionData()
                {
                    Time = time,
                    onClick = (timeSelected) => ChooseOption(option, timeSelected)
                });
                timeOptions.Add(option);
            }

            int chosenTimeIndex = Array.IndexOf(_times, _workoutsModel.ChosenTime);
            if (chosenTimeIndex == -1)
                chosenTimeIndex = 0;

            SetOption(_times[chosenTimeIndex]);
        }

        private void SetOption(int chosenTime)
        {
            _workoutsModel.ChosenTime = chosenTime;
            for (int i = 0; i < _times.Length; i++)
            {
                if (chosenTime == _times[i])
                {
                    ChooseOption(timeOptions[i], chosenTime);
                    return;
                }
            }
            ChooseOption(timeOptions[0], _times[0]);
        }

        private void ChooseOption(TimeOption option, int timeSelected)
        {
            _workoutsModel.ChosenTime = timeSelected;
            _dataRepository.Save(_workoutsModel, true);
            foreach (var item in timeOptions)
            {
                if(ReferenceEquals(option, item))
                    item.Select();
                else
                    item.DeSelect();
            }
        }
    }
}
