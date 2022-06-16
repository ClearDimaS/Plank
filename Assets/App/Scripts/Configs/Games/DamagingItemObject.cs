using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.Configs.Games
{
    [CreateAssetMenu(menuName = "Configs/Games/DamagingItem", fileName = "DamagingItem", order = 0)]
    public class DamagingItemObject : ScriptableObject
    {
        public int Damage;
    }
}
