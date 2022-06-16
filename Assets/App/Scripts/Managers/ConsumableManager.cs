using System.Collections;
using System.Collections.Generic;
using Smartplank.Scripts.Models;
using UnityEngine;
using Zenject;

namespace Smartplank.Scripts.Games.Managers
{
    public class ConsumableManager : MonoBehaviour
    {
        [SerializeField] private ConsumableItem[] consumableItems;
        [SerializeField] private ConsumableItemLong[] consumableItemsLong;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private bool spawnOnlyOne;
        [SerializeField] private Vector3 direction = Vector3.down;

        private DataRepository _dataRepository;
        private GamesSharedModel _gamesModel;
     
        private GameObject lastSpawned;

        [Inject]
        void Construct(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        private void Start()
        {
            _gamesModel = _dataRepository.GetData<GamesSharedModel>();
            StartCoroutine(nameof(SpawnCoroutine));
        }

        private IEnumerator SpawnCoroutine()
        {
            if (consumableItems.Length == 0 && consumableItemsLong.Length == 0)
                yield break;

            while (true)
            {
                if (!spawnOnlyOne)
                    yield return new WaitForSeconds(UnityEngine.Random.Range(_gamesModel.ConsumableSpawnOptions.MinSpawnPause,
                    _gamesModel.ConsumableSpawnOptions.MaxSpawnPause));
                else
                    yield return null;
                if (spawnOnlyOne && lastSpawned != null)
                    continue;

                if (consumableItems.Length != 0)
                    Spawn(consumableItems);
                else if (consumableItemsLong.Length != 0)
                    Spawn(consumableItemsLong);
                else
                    yield break;
            }
        }

        private void Spawn(ConsumableItem[] consumableItems)
        {
            var consumable = consumableItems[UnityEngine.Random.Range(0, consumableItems.Length)];
            var spawn = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            var newConsumable = Instantiate(consumable, spawn);
            lastSpawned = newConsumable.gameObject;

            if (newConsumable.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.down * UnityEngine.Random.Range(_gamesModel.ConsumableSpawnOptions.MinSpeed,
                    _gamesModel.ConsumableSpawnOptions.MaxSpeed);
            }
        }

        private void Spawn(ConsumableItemLong[] consumableItems)
        {
            var consumable = consumableItems[UnityEngine.Random.Range(0, consumableItems.Length)];
            var spawn = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            var newConsumable = Instantiate(consumable, spawn);
            lastSpawned = newConsumable.gameObject;

            if (newConsumable.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = direction * UnityEngine.Random.Range(_gamesModel.ConsumableSpawnOptions.MinSpeed,
                    _gamesModel.ConsumableSpawnOptions.MaxSpeed);
            }
        }
    }
}
