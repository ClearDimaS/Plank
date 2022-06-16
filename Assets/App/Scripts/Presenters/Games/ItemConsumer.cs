using System;
using System.Collections;
using System.Collections.Generic;
using Smartplank.Scripts.Models;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts.Games
{
    public class ItemConsumer : MonoBehaviour
    {
        [SerializeField] private Player player;

        private DataRepository _dataRepository;

        private float longStarted = 10000f;
        private ConsumableItemLong longItem;

        [Inject]
        public void Construct(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        private void Awake()
        {
            player.onDeath += SaveScores;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IConsumable consumable))
            {
                if (consumable.GetType() == typeof(ConsumableItemLong))
                {
                    longStarted = Time.time;
                    longItem = consumable as ConsumableItemLong;
                }
                else
                {
                    consumable.Consume(player.gameObject);
                }
            }

            if (other.gameObject.TryGetComponent(out IDamager damager))
            {
                damager.DealDamage(player);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IConsumable consumable))
            {
                if (consumable.GetType() == typeof(ConsumableItemLong))
                {
                    longStarted = Mathf.Infinity;
                    longItem = null;
                }
            }
        }

        private void Update()
        {
            if (Time.time - longStarted > 2f)
            {
                if (longItem != null)
                    longItem.Consume(player.gameObject);
            }
        }

        private void SaveScores()
        {
            var scores = _dataRepository.GetData<UserScoresModel>();
            scores.gamesCount++;
            _dataRepository.Save(scores, true);
        }

        public void AddScores(int rewardPoints)
        {
            var scores = _dataRepository.GetData<UserScoresModel>();
            scores.scoreCount += rewardPoints;
        }
    }
}
