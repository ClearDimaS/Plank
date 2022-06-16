using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.Configs
{
    [CreateAssetMenu(fileName = "NetworkConfig", menuName = "Configs/NetworkConfig", order = 0)]
    public class NetworkConfig : ScriptableObject
    {
        [SerializeField] private string appUrl;
        public string AppUrl => appUrl;
    }
}
