using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Smartplank.Scripts.Configs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Smartplank.Scripts.Presenters.Panels
{
    public class MainMenuPanel : PanelBase
    {
        [SerializeField] private Button highscoreButton;
        [SerializeField] private Button webButton;
        [SerializeField] private Button instructionsButton;
        [SerializeField] private Button workOutCalendarButton;
        [SerializeField] private Button gamesButton;
        [SerializeField] private Button workoutCourseButton;
        [SerializeField] private RadialLayoutGroup radialLayoutGroup;

        private PanelsController _panelsController;
        private NetworkConfig _networkConfig;
        private GraphicRaycaster graphicRaycaster;

        [Inject]
        private void Construct(PanelsController panelsController, NetworkConfig networkConfig)
        {
            _panelsController = panelsController;
            _networkConfig = networkConfig;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var target = radialLayoutGroup.GetTargetSegment(Input.mousePosition);

                var raycastResults = new List<RaycastResult>();

                graphicRaycaster.Raycast(new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                }, raycastResults);

                Debug.Log($" {raycastResults[0].gameObject.transform.parent.name}     {target.parent.name}");
                if (raycastResults[0].gameObject.transform.parent.GetInstanceID() != target.parent.GetInstanceID() &&
                    raycastResults[0].gameObject.transform.parent.parent.GetInstanceID() != target.parent.GetInstanceID())
                    return;

                var button = target.GetComponentInChildren<Button>();
                if (ReferenceEquals(button, highscoreButton))
                    _panelsController.OpenPanel<HighScorePanel>();
                if (ReferenceEquals(button, webButton))
                    Application.OpenURL($"{_networkConfig.AppUrl}");
                if (ReferenceEquals(button, instructionsButton))
                    _panelsController.OpenPanel<InstructionsPanel>();
                if (ReferenceEquals(button, workOutCalendarButton))
                    _panelsController.OpenPanel<WorkOutCalendarPanel>();
                if (ReferenceEquals(button, gamesButton))
                    _panelsController.OpenPanel<GamesPanel>();
                if (ReferenceEquals(button, workoutCourseButton))
                    _panelsController.OpenPanel<WorkOutCoursePanel>();
            }
        }

        public override void OnShowing()
        {
            base.OnShowing();
        }
    }
}
