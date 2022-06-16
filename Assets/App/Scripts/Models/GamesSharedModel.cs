using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Smartplank.Scripts.Configs;

namespace Smartplank.Scripts.Models
{
    [Serializable]
    public class GamesSharedModel
    {
        public string ChosenDifficultyKey;
        public GameData ChosenGame;

        public EmenySpawnOptions EnemySpawnOptions;
        public ConsumableSpawnOptions ConsumableSpawnOptions;
    }
}

