using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Smartplank.Scripts.Configs;
using Smartplank.Scripts.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Smartplank.Scripts.Games
{
    public class ChooseGamePresenter : MonoBehaviour
    {
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private GameOption gameOptionPrefab;
        [SerializeField] private GameObject gamesBorderPrefab;
        [SerializeField] private Transform gameOptionsParent;

        private List<GameOption> gameOptions = new List<GameOption>();

        private GamesConfig _gamesConfig;
        private GamesSharedModel _gamesSharedModel;
        private DataRepository _dataRepository;

        private GameData[] _games => _gamesConfig.Games;
        private Transform centerTransform;

        private bool hasStarted;

        [Inject]
        private void Construct(GamesConfig gamesConfig, DataRepository dataRepository)
        {
            _gamesConfig = gamesConfig;
            _dataRepository = dataRepository;
        }

        private void Start()
        {
            _gamesSharedModel = _dataRepository.GetData<GamesSharedModel>();
            Instantiate(gamesBorderPrefab, gameOptionsParent);
            var onClickSignal = new Signal<GameOption>();
  
            foreach (var game in _games)
            {
                var option = Instantiate(gameOptionPrefab, gameOptionsParent);
                option.Init(new GameOption.OptionData()
                {
                    gameSprite = game.Sprite,
                    onClick = onClickSignal // => ChooseOption(option, game)
                });
                gameOptions.Add(option);
            }
            Instantiate(gamesBorderPrefab, gameOptionsParent);

            onClickSignal.Event += (option) => ChooseOption(option);

            int chosenGameIndex = Array.IndexOf(_games, _gamesSharedModel.ChosenGame);
            if (chosenGameIndex == -1)
                chosenGameIndex = 0;

            onClickSignal.Event?.Invoke(gameOptions[chosenGameIndex]);

            scroll.onValueChanged.AddListener(SetCenterElement);

            if (!hasStarted)
            {
                hasStarted = true;
                Invoke(nameof(SetScrollPosition), 0f);
            }
        }

        private void OnEnable()
        {
            if(hasStarted)
                SetScrollPosition();
        }

        private void SetScrollPosition()
        {
            var ttlWidth = scroll.content.rect.width;
            var borderWidth = gamesBorderPrefab.GetComponent<RectTransform>().rect.width;
            scroll.horizontalNormalizedPosition = 2 * borderWidth * 0.9f / ttlWidth;
        }

        private void SetCenterElement(Vector2 scrollValue)
        {

            var center = gameOptions.OrderBy(x => Mathf.Abs(RectTransformUtility.WorldToScreenPoint(null, x.transform.position).x / Screen.width - 0.5f)).First();
            if (ReferenceEquals(centerTransform, center.transform))
                return;
            else
                centerTransform = center.transform;
            foreach (var item in gameOptions)
            {
                if (ReferenceEquals(item, center))
                    item.SetCentered();
                else
                    item.SetDeCentered();
            }

        }

        private void ChooseOption(GameOption option)
        {
            var gameSelected = _games[gameOptions.IndexOf(option)];
            _gamesSharedModel.ChosenGame = gameSelected;

            _dataRepository.Save(_gamesSharedModel, true);

            Debug.Log($"choosing game");

            if (firstLoad)
                firstLoad = false;
            else
                LoadGame();
        }

        private bool firstLoad = true;

        private void LoadGame()
        {
            SceneManager.LoadScene(_gamesSharedModel.ChosenGame.sceneIndex);
        }
    }
}
