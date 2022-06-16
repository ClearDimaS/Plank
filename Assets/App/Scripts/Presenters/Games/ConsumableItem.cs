using System.Collections;
using System.Collections.Generic;
using Smartplank.Scripts.Configs.Games;
using Smartplank.Scripts.Models;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts.Games
{
    public class ConsumableItem : MonoBehaviour, IConsumable
    {
        [SerializeField] private ConsumableItemObject consumableItemObject;

        public void Consume(GameObject target)
        {
            if (target.TryGetComponent(out ItemConsumer consumer))
            {
                consumer.AddScores(consumableItemObject.RewardPoints);
                Destroy(gameObject);
            }
        }
    }
}
