using System;
using System.Collections;
using System.Collections.Generic;
using RSG;
using Smartplank.Scripts.AnimationTransitions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Smartplank.Scripts.Presenters.Panels
{
    /// <summary>
    /// panels here impelemt roles of presenters, NOT VIEWS, NOT CONTROLLER, NOT MANAGERS OR ETC
    /// IT IS REALLY NOT DESIRED PRESENTERS TO KNOW ABOUT EACH OTHER
    /// </summary>
    public abstract class PanelBase : MonoBehaviour, IPanel
    {
        [SerializeField] protected Button closeButton;

        public ITransitionAnimation panelAnimator { get; private set; }
        public bool Initialized { get; private set; }

        protected void Awake()
        {
            Initialized = false;

            panelAnimator = GetComponent<ITransitionAnimation>();
        }

        private void Start()
        {
            if (Initialized)
                return;
            OnAwake();
        }

        protected virtual void OnAwake() { } // TODO: logic here could be replaced with start???

        public virtual void OnInit()
        {
            Initialized = true;
        }

        public virtual void SetUp() { }

        public virtual void OnShowing() { }

        public virtual void OnShown() { }

        public virtual void OnHidding() { }

        public virtual void OnHidden() { }

        public Promise Show()
        {
            if (!Initialized)
            {
                OnAwake();
                Initialized = true;
            }

            var promise = new Promise();
            OnShowing();
            if (panelAnimator != null)
            {
                panelAnimator.AnimateShow().Done(() =>
                {
                    promise.Resolve();
                    OnShown();
                });
            }
            else
            {
                promise.Resolve();
                OnShown();
            }
            return promise;
        }

        public Promise Hide()
        {
            var promise = new Promise();
            OnHidding();
            if (panelAnimator != null)
            {
                panelAnimator.AnimateHide().Done(() =>
                {
                    promise.Resolve();
                    OnHidden();
                });
            }
            else
            {
                promise.Resolve();
                OnHidden();
            }
            return promise;
        }
    }

    public abstract class PanelBase<T> : PanelBase, IPanel<T>
    {
        protected T initData;

        protected T setUpData;

        public void Init(T initData)
        {
            this.initData = initData;
        }

        public virtual void SetUp(T setUpData)
        {
            this.setUpData = setUpData;
        }

        protected void SetUp() { }
    }
}
