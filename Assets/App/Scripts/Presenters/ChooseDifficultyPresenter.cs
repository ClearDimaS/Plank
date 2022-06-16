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
    public class ChooseDifficultyPresenter : MonoBehaviour
    {
        [SerializeField] private DifficultyOption difficultyPrefab;
        [SerializeField] private Transform difficultyOptionsParent;
        [SerializeField] private string[] difficulties; // might replace with config


        private GamesSharedModel _gamesSharedModel;
        private DataRepository _dataRepository;
        private GameDifficultiesConfig _gameDifficultiesConfig;

        private List<DifficultyOption> difficultyOptions = new List<DifficultyOption>();

        private GameDifficulty[] _difficulties => _gameDifficultiesConfig.GameDifficulties;

        [Inject]
        private void Construct(DataRepository dataRepository, GameDifficultiesConfig gameDifficultiesConfig)
        {
            _dataRepository = dataRepository;
            _gameDifficultiesConfig = gameDifficultiesConfig;
        }

        private void Start()
        {
            _gamesSharedModel = _dataRepository.GetData<GamesSharedModel>();
            foreach (var diffuculty in _difficulties)
            {
                var option = Instantiate(difficultyPrefab, difficultyOptionsParent);
                option.Init(new DifficultyOption.OptionData()
                {
                    difficultyKey = diffuculty.KeyName,
                    onClick = (difficultySelected) => ChooseOption(option)
                });
                difficultyOptions.Add(option);
            }

            int chosenTimeIndex = Array.IndexOf(_difficulties, _gamesSharedModel.ChosenDifficultyKey);
            if (chosenTimeIndex == -1)
                chosenTimeIndex = 0;

            ChooseOption(difficultyOptions[chosenTimeIndex]);
        }

        private void ChooseOption(DifficultyOption option)
        {
            var difficultyIndex = difficultyOptions.IndexOf(option);

            _gamesSharedModel.ChosenDifficultyKey = _difficulties[difficultyIndex].KeyName;
            _gamesSharedModel.EnemySpawnOptions = _difficulties[difficultyIndex].EnemySpawnOptions;
            _gamesSharedModel.ConsumableSpawnOptions = _difficulties[difficultyIndex].ConsumableSpawnOptions;

            _dataRepository.Save(_gamesSharedModel, true);

            foreach (var item in difficultyOptions)
            {
                if(ReferenceEquals(option, item))
                    item.Select();
                else
                    item.DeSelect();
            }
        }
    }
}
