using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Smartplank.Localization;
using Zenject;
using Smartplank.Scripts.AnimationTransitions;

namespace Smartplank.Scripts.UI
{
    public class LanguageSelection : MonoBehaviour
    {
        [SerializeField] private Button enButton;
        [SerializeField] private Button ruButton;

        [SerializeField] private GameObject languageShownGO;
        [SerializeField] private GameObject languageHiddenGO;

        [SerializeField] private Button languageChangeBtn;

        private bool isShown;
        private ITransitionAnimation transitionAnimation;
        private LocalizationManager _localizationManager;
        private List<ButtonLanguge> buttonLanguages;

        [Inject]
        private void Construct(LocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
        }

        private void Awake()
        {
            transitionAnimation = languageShownGO.GetComponent<ITransitionAnimation>();
            if (transitionAnimation == null)
                transitionAnimation = languageShownGO.AddComponent<SimplePanelAnimator>();
        }

        private void Start()
        {
            languageChangeBtn.onClick.AddListener(() =>
            {
                if (isShown)
                    Hide(languageChangeBtn);
                else
                    Show();
            });
            buttonLanguages = new List<ButtonLanguge>()
            {
                new ButtonLanguge(AvailableLanguages.English, enButton, _localizationManager, Hide),
                new ButtonLanguge(AvailableLanguages.Russian, ruButton, _localizationManager, Hide),
            };

            SetUp();
        }

        private void SetUp()
        {
            foreach (var item in buttonLanguages)
            {
                if (item.AvailableLanguage == _localizationManager.CurrentLanguage)
                    Hide(item.Button);
            }
        }

        private void Hide(Button button)
        {
            languageChangeBtn.image.sprite = button.image.sprite;
            transitionAnimation.AnimateHide().Done(() =>
            {
                isShown = false;
                languageShownGO.SetActive(isShown);
                languageHiddenGO.SetActive(!isShown);
            });
        }

        private void Show()
        {
            transitionAnimation.AnimateShow();
            isShown = true;
            languageShownGO.SetActive(isShown);
            languageHiddenGO.SetActive(!isShown);
        }

        private class ButtonLanguge
        {
            public Button Button => button;
            private Button button;
            public AvailableLanguages AvailableLanguage { get; private set; }

            public ButtonLanguge(AvailableLanguages availableLanguage, Button button, LocalizationManager localizationManager,
                Action<Button> onClickExtraAction)
            {
                AvailableLanguage = availableLanguage;
                this.button = button;

                button.onClick.AddListener(() =>
                {
                    localizationManager.SelectLanguage(availableLanguage);
                    onClickExtraAction?.Invoke(button);
                });
            }
        }
    }
}