using System.Collections;
using System.Collections.Generic;
using Smartplank.Localization;
using Smartplank.Scripts.Configs;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts.Installers
{
    [CreateAssetMenu(fileName = "ProjectConfigsInstaller", menuName = "Installers/ProjectConfigsInstaller")]
    public class ProjectConfigsInstaller : ScriptableObjectInstaller<ProjectConfigsInstaller>
    {
        [SerializeField] private LocalizationTexts localizationTexts;
        [SerializeField] private NetworkConfig networkConfig;
        [SerializeField] private WorkoutsConfig workoutsConfig;
        [SerializeField] private Day30ChallengeConfig day30ChallengeConfig;
        [SerializeField] private InstructionsConfig instructionsConfig;
        [SerializeField] private GamesConfig gamesConfig;
        [SerializeField] private GameDifficultiesConfig difficultiesConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(localizationTexts).AsSingle().NonLazy();
            Container.BindInstance(networkConfig).AsSingle().NonLazy();
            Container.BindInstance(workoutsConfig).AsSingle().NonLazy();
            Container.BindInstance(day30ChallengeConfig).AsSingle().NonLazy();
            Container.BindInstance(instructionsConfig).AsSingle().NonLazy();
            Container.BindInstance(gamesConfig).AsSingle().NonLazy();
            Container.BindInstance(difficultiesConfig).AsSingle().NonLazy();
        }
    }
}
