using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Smartplank.Scripts
{
    public class WorkoutAction : MonoBehaviour
    {
        [SerializeField] private Image blueImage;
        [SerializeField] private Button goButton;
        [SerializeField] private TMP_Text timeLeftText;

        private int lastTime = -1;
        private Color originalColor;
        private Color startColor;
        private void Awake()
        {
            originalColor = Color.Lerp(Color.blue, Color.white, 0.6f);
            var col = Color.white;
            col.a = 0;
            startColor = col;
        }

        private void OnEnable()
        {
            blueImage.transform.localScale = Vector3.zero;
        }

        public void SetUp(Action onClick)
        {
            goButton.onClick.AddListener(() => onClick?.Invoke());
            blueImage.transform.localScale = Vector3.zero;
        }

        public void ClearListeners()
        {
            goButton.onClick.RemoveAllListeners();
        }

        public void SetTime(float time, bool animate = true)
        {
            if (lastTime != (int)time)
            {
                blueImage.transform.localScale = Vector3.zero;
                lastTime = (int)time;

                if (animate)
                {
                    Animate();
                }
            }
            timeLeftText.text = ((int)time).ToString();
        }

        private void Animate()
        {
            float startTime = Time.time;
            float duration = 0.8f;
            blueImage.transform.DOScale(Vector3.one, duration).OnUpdate(() =>
            {
                float val = Time.time - startTime;
                if (val * duration > 0.5f * duration)
                    val = (1 - val);
                blueImage.color = Color.Lerp(startColor, originalColor, val);
            }).OnComplete(() =>
            {
                blueImage.transform.localScale = Vector3.zero;
            });
        }
    }
}
