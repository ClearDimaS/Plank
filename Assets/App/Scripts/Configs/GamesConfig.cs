using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Smartplank.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GamesConfig", menuName = "Configs/Games/Games", order = 1)]
    public class GamesConfig : ScriptableObject
    {
        [SerializeField] private GameData[] games;
        public GameData[] Games => games;
    }

    [System.Serializable]
    public class GameData
    {
        [JsonIgnore]
        public Sprite Sprite;
        public int sceneIndex;
    }
}
