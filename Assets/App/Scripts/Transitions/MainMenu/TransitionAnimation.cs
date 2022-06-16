using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts.AnimationTransitions
{
    public class TransitionAnimation : TransitionAnimationBase
    {
        [SerializeField] private const int startPoint = +1500;
        [SerializeField] private const int endPoint = 0;

        [SerializeField] protected AnimationCurve openingAnimationCurve;
        [SerializeField] protected AnimationCurve closingAnimationCurve;

        protected float totalTime;
        protected RectTransform rectTransform;
        protected CanvasRenderer[] canvasRenderers;
        protected CanvasGroup canvasGroup;

        [Inject]
        private void Construct()
        {
            totalTime = openingAnimationCurve.keys[openingAnimationCurve.keys.Length - 1].time;
            rectTransform = gameObject.GetComponent<RectTransform>();
            canvasRenderers = gameObject.GetComponentsInChildren<CanvasRenderer>();
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
        }

        protected override void DoShow()
        {
            rectTransform.DOAnchorPosX(startPoint, 0);
            rectTransform.DOAnchorPosX(endPoint, totalTime).SetEase(animCurve: openingAnimationCurve);
            canvasGroup.DOFade(0, 0);
            canvasGroup.DOFade(1, totalTime).SetEase(animCurve: openingAnimationCurve).onComplete += () => ResolveAndStop(false);
        }

        protected override void DoHide()
        {
            rectTransform.DOAnchorPosX(endPoint, 0);
            rectTransform.DOAnchorPosX(startPoint, totalTime).SetEase(animCurve: closingAnimationCurve);
            canvasGroup.DOFade(1, 0);
            canvasGroup.DOFade(0, totalTime).SetEase(animCurve: closingAnimationCurve).onComplete += () => ResolveAndStop(false);
        }
    }

}