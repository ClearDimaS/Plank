using System;
using System.Collections;
using System.Collections.Generic;
using RSG;
using TMPro;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts.Presenters.Panels
{
    public class LoadingGamePanel : PanelBase<LoadingGamePanel.TimerData>
    {
        public struct TimerData
        {
            public int duration;
            public Promise promise;
        }

        [SerializeField] private TMP_Text timerText;
        private PanelsController _panelsController;
        private Promise promise;

        [Inject]
        private void Construct(PanelsController panelsController)
        {
            _panelsController = panelsController;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            closeButton.onClick.AddListener(GoBack);
        }

        private void GoBack()
        {
            promise.Reject(new Exception());
            StopAllCoroutines();
            _panelsController.CloseLastPanel();
        }

        public override void SetUp(TimerData timerData)
        {
            base.SetUp(timerData);
            Debug.Log($"Set up loading...");
            promise = timerData.promise;
            timerText.text = timerData.duration.ToString();
            CancelInvoke();
            Invoke(nameof(StartTimer), 0.3f);
        }

        private void StartTimer()
        {
            if (gameObject.activeInHierarchy)
            {
                StopAllCoroutines();
                StartCoroutine(nameof(LaunchTimer));
            }
            else
                Invoke(nameof(StartTimer), 0.3f); 
        }

        private IEnumerator LaunchTimer()
        {
            float startTime = Time.time;
            while (Time.time - startTime < setUpData.duration)
            {
                yield return null;
                float timeLeft = setUpData.duration - (Time.time - startTime);
                timerText.text = Mathf.RoundToInt(timeLeft).ToString();
            }
            promise.Resolve();
        }

        public override void OnShowing()
        {
            base.OnShowing();
        }
    }
}
