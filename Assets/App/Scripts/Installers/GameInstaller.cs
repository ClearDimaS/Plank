using System;
using System.Collections;
using System.Collections.Generic;
using Smartplank.Localization;
using Smartplank.Scripts.Models;
using Smartplank.Scripts.Presenters.Panels;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LocalizationManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DataRepository>().AsSingle().NonLazy();
        }
    }
}
