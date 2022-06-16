using System;
using System.Collections;
using System.Collections.Generic;
using Smartplank.Scripts.Configs;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts.Presenters.Panels
{
    public class InstructionsPanel : PanelBase
    {
        [SerializeField] private InstructionHandler instructionHandlerPrefab;
        [SerializeField] private Transform instructionsParent;

        private PanelsController _panelsController;
        private InstructionsConfig _instructionsConfig;

        [Inject]
        private void Construct(PanelsController panelsController, InstructionsConfig instructionsConfig)
        {
            _panelsController = panelsController;
            _instructionsConfig = instructionsConfig;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            closeButton.onClick.AddListener(_panelsController.OpenMainMenu);

            CreateInstructions();
        }

        private void CreateInstructions()
        {
            foreach (var instruction in _instructionsConfig.Instructions)
            {
                var newInstruction = Instantiate(instructionHandlerPrefab, instructionsParent);
                newInstruction.SetUp(new InstructionHandler.SetUpData()
                {
                    instructionData = instruction
                });
            }
        }
    }
}
