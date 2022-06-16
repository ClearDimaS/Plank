
using RSG;
using UnityEngine;

namespace Smartplank.Scripts.AnimationTransitions
{
    [RequireComponent(typeof(RectTransform))]
    public class SimplePanelAnimator : TransitionAnimationBase
    {
        private struct State
        {
            public float alpha;
            public float scale;
        }

        [SerializeField] private bool animateScale = true;
        [SerializeField] private bool animateAlpha = true;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        private State visibleState = new State { alpha = 1f, scale = 1f };
        private State hiddenState = new State { alpha = 0f, scale = 1.2f };

        private float currentState = 0f;
        protected float goalState = 0f;
        private const float animationSpeed = 15f;


        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            ApplyState();
        }

        void Update()
        {
            if (Mathf.Approximately(currentState, goalState) == true)
            {
                ResolveAndStop(false);
            }
            else
            {
                currentState = Mathf.MoveTowards(currentState, goalState, Time.unscaledDeltaTime * animationSpeed);
                ApplyState();
            }
        }

        protected override void DoShow()
        {
            goalState = 1f;
        }

        protected override void DoHide()
        {
            goalState = 0f;
        }

        private void ApplyState()
        {
            State state = GetCurrentState();
            if (animateAlpha == true)
            {
                canvasGroup.alpha = state.alpha;
            }
            if (animateScale == true)
            {
                rectTransform.localScale = Vector3.one * state.scale;
            }
        }

        private State GetCurrentState()
        {
            return new State
            {
                alpha = Mathf.Lerp(hiddenState.alpha, visibleState.alpha, currentState),
                scale = Mathf.Lerp(hiddenState.scale, visibleState.scale, currentState),
            };
        }
    }
}
