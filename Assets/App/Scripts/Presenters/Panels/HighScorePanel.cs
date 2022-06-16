using System.Collections;
using System.Collections.Generic;
using Smartplank.Scripts.Models;
using TMPro;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts.Presenters.Panels
{
    public class HighScorePanel : PanelBase
    {
        [SerializeField] private TMP_Text scoresText;
        [SerializeField] private TMP_Text gamesText;

        private PanelsController _panelsController;
        private DataRepository _dataRepository;
        private UserScoresModel _userScoresModel;

        [Inject]
        private void Construct(PanelsController panelsController, DataRepository dataRepository)
        {
            _panelsController = panelsController;
            _dataRepository = dataRepository;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _userScoresModel = _dataRepository.GetData<UserScoresModel>();
            closeButton.onClick.AddListener(_panelsController.OpenMainMenu);
        }

        public override void OnShowing()
        {
            base.OnShowing();
            scoresText.text = _userScoresModel.scoreCount.ToString();
            gamesText.text = _userScoresModel.gamesCount.ToString();
        }
    }
}
