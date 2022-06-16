using System.Collections;
using System.Collections.Generic;
using RSG;
using UnityEngine;

namespace Smartplank.Scripts.Presenters.Panels
{
    public interface IPanel
    {
        public void OnInit();

        public void OnShowing();

        public void OnShown();

        public void OnHidding();

        public void OnHidden();

        public void SetUp();
    }

    public interface IPanel<T> : IPanel, IInitable<T>, ISetupable<T>
    {

    }
}
