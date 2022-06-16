using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Smartplank.Scripts
{
    public class RadialLayoutGroup : LayoutGroup
    {
        [SerializeField] private Sprite segmentBackground; // cirvle for circular menu, quD FOR FULL SCREEN SEGMENTATION
        [SerializeField] private float paddingBetweenSegments;
        [SerializeField] private float[] angles;
        [SerializeField] private bool useEvenAgnles;

        private Vector2 middleScreenPoint;

        protected override void Start()
        {
            middleScreenPoint = new Vector2(Screen.width, Screen.height) / 2f;
        }

        public Transform GetTargetSegment(Vector2 screenPoint)
        {
            var dirFromCenter = (screenPoint - middleScreenPoint).normalized;

            var targetAngle = Vector2.SignedAngle(Vector2.down, dirFromCenter);
            if (targetAngle < 0)
                targetAngle += 360f;

            var angleSum = -(360f - transform.localRotation.eulerAngles.z);

            for (int i = 0; i < angles.Length; i++)
            {
                angleSum += angles[i];

                if (angleSum >= targetAngle)
                {
                    return transform.GetChild(i);
                }
            }
            return transform.GetChild(0) ;
        }

        public override void CalculateLayoutInputVertical()
        {

        }

        public override void SetLayoutHorizontal()
        {

        }

        public override void SetLayoutVertical()
        {
            float angleStep = 360f / rectChildren.Count;
            var _angles = angles;
            if (useEvenAgnles)
            {
                if (_angles.Length != rectChildren.Count)
                {
                    _angles = new float[rectChildren.Count];
                    for (int i = 0; i < rectChildren.Count; i++)
                    {
                        _angles[i] = angleStep * (i + 0.5f);
                    }
                }
            }

            float anglesSum = 0f;
            for (int i = 0; i < rectChildren.Count; i++)
            {
                var rect = rectChildren[i];
                rect.pivot = Vector2.one / 2f;
                var rot1 = Quaternion.Euler(Vector3.forward * (anglesSum + _angles[i]));
                var rot2 = Quaternion.Euler(Vector3.forward * (360f / _angles.Length * (i + 0.5f))); // just step * i
                anglesSum += _angles[i];

                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.sizeDelta = Vector2.zero;
                rect.localRotation = rot1;
                rect.localPosition = rot2 * Vector3.down * paddingBetweenSegments;

                var fillAmmount = ((_angles[i] - Mathf.RoundToInt(_angles[i] / angleStep) * angleStep) + angleStep) / 360f;

                SetSubRects(rect, fillAmmount);
            }

            if (rectChildren.Count == 1)
            {
                var rect = rectChildren[0];
                rect.pivot = Vector2.one / 2f;
                rect.localRotation = Quaternion.identity;
                rect.localPosition = Vector3.zero;
            }
        }

        private void SetSubRects(RectTransform rect, float fillAmmount)
        {
            var img = rect.GetComponent<Image>();
            if (img == null)
                img = rect.gameObject.AddComponent<Image>();

            img.type = Image.Type.Filled;
            img.fillMethod = Image.FillMethod.Radial360;
            img.fillAmount = fillAmmount;
            img.sprite = segmentBackground;
            img.preserveAspect = true;

            if (rect.childCount == 0)
                return;
        }
    }
}
