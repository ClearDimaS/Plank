using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts.Installers
{
    public class AdditionalProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameObject[] additionalDontDestroyPrefabs;

        public override void InstallBindings()
        {
            var dontDestoryParent = new GameObject("DontDestroyPrefabsParent");
            DontDestroyOnLoad(dontDestoryParent);
            foreach (var item in additionalDontDestroyPrefabs)
            {
                Instantiate(item, dontDestoryParent.transform);
            }

            Container.Bind<LocalCacheManager>().
                FromNewComponentOnNewGameObject().UnderTransform(dontDestoryParent.transform).AsSingle().NonLazy();
        }
    }
}
