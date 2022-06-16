using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Smartplank.Scripts.Presenters.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Smartplank.Scripts.Games
{
    public class GameOption : MonoBehaviour, IInitable<GameOption.OptionData>
    {
        public struct OptionData
        {
            public Sprite gameSprite;
            public Signal<GameOption> onClick;
        }

        [SerializeField] private Image gameIcon;
        [SerializeField] private Button chooseButton;
        [SerializeField] private GameObject centeredGO;
        [SerializeField] private float selectTime = 1f;

        private OptionData _initData;

        private void Awake()
        {
            centeredGO.transform.localScale = Vector3.zero;
        }

        public void Init(OptionData initData)
        {
            _initData = initData;
            gameIcon.sprite = _initData.gameSprite;
            chooseButton.onClick.RemoveAllListeners();
            chooseButton.onClick.AddListener(() => _initData.onClick.Event?.Invoke(this));
            _initData.onClick.Event += Clicked;
        }

        private void Clicked(GameOption option)
        {
            if (ReferenceEquals(option, this))
            {
                Select();
            }
            else
            {
                DeSelect();
            }
        }

        private void Select()
        {

        }

        private void DeSelect()
        {

        }

        public void SetCentered()
        {
            centeredGO.transform.DOKill();
            float time = (1f - centeredGO.transform.localScale.x) * selectTime;
            centeredGO.transform.DOScale(Vector3.one, time).SetEase(Ease.InCubic);
        }

        public void SetDeCentered()
        {
            centeredGO.transform.DOKill();
            float time = (centeredGO.transform.localScale.x - 0f) * selectTime;
            centeredGO.transform.DOScale(Vector3.zero, time).SetEase(Ease.InCubic);
        }
    }

    public class Signal<T>
    {
        public Action<T> Event;
    }
}
