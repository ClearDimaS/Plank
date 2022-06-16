using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts.Presenters.Panels
{
    public class GamesPanel : PanelBase
    {
        private PanelsController _panelsController;

        [Inject]
        private void Construct(PanelsController panelsController)
        {
            _panelsController = panelsController;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            closeButton.onClick.AddListener(_panelsController.OpenMainMenu);
        }
    }
}
