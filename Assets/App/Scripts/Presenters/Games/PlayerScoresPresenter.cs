using System.Collections;
using System.Collections.Generic;
using Smartplank.Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Smartplank.Scripts.Games
{
    public class PlayerScoresPresenter : MonoBehaviour
    {
        [SerializeField] private Button leaveButton;
        [SerializeField] private TMP_Text scoresText;

        private DataRepository _dataRepository;
        private UserScoresModel _userScoresModel;

        [Inject]
        private void Construct(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        void Awake()
        {
            _userScoresModel = _dataRepository.GetData<UserScoresModel>();
            leaveButton.onClick.AddListener(GoMenu);
        }

        private void Update()
        {
            scoresText.text = _userScoresModel.scoreCount.ToString();
        }

        private void GoMenu()
        {
            SaveScores();
            SceneManager.LoadScene(0);
        }

        private void SaveScores()
        {
            var scores = _dataRepository.GetData<UserScoresModel>();
            scores.gamesCount++;
            _dataRepository.Save(scores, true);
        }
    }
}
