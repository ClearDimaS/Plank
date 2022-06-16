using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.Configs.Games
{
    [CreateAssetMenu(menuName = "Configs/Games/ConsumableItem", fileName = "ConsumableItem", order = 0)]
    public class ConsumableItemObject : ScriptableObject
    {
        public int RewardPoints;
    }
}
