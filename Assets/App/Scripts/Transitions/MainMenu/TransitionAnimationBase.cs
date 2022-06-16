using DG.Tweening;
using RSG;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts.AnimationTransitions
{
    public abstract class TransitionAnimationBase : MonoBehaviour, ITransitionAnimation
    {
        protected Promise onCompleteTransition;

        public Promise AnimateShow()
        {
            ResolveAndStop(true);
            onCompleteTransition = new Promise();
            onCompleteTransition.Done(() => gameObject.SetActive(true));
            DoShow();
            return onCompleteTransition;
        }

        public Promise AnimateHide()
        {
            ResolveAndStop(true);
            onCompleteTransition = new Promise();
            onCompleteTransition.Done(() => gameObject.SetActive(false));
            DoHide();
            return onCompleteTransition;
        }

        protected void ResolveAndStop(bool interrupted)
        {
            if (onCompleteTransition != null)
            {
                if (interrupted)
                {
                    onCompleteTransition.Reject(new System.Exception("Hide was cancelled"));
                }
                else
                {
                    onCompleteTransition.Resolve();
                }
                onCompleteTransition = null;
            }
        }

        protected virtual void DoShow()
        {

        }

        protected virtual void DoHide()
        {

        }
    }
}

