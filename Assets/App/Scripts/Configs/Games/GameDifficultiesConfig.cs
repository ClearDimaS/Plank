using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Smartplank.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Games/Difficulties", fileName = "GameDifficulties", order = 0)]
    public class GameDifficultiesConfig : ScriptableObject
    {
        public GameDifficulty[] GameDifficulties;
    }

    [Serializable]
    public class GameDifficulty
    {
        public string KeyName;

        public EmenySpawnOptions EnemySpawnOptions;
        public ConsumableSpawnOptions ConsumableSpawnOptions;
    }

    [Serializable]
    public class EmenySpawnOptions
    {
        public float MinSpeed = 0.5f;
        public float MaxSpeed = 0.5f;
        public float MinSpawnPause;
        public float MaxSpawnPause;
    }

    [Serializable]
    public class ConsumableSpawnOptions
    {
        public float MinSpeed = 0.5f;
        public float MaxSpeed = 0.5f;
        public float MinSpawnPause;
        public float MaxSpawnPause;
    }
}
