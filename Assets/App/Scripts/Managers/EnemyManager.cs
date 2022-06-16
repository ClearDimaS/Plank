using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Smartplank.Scripts.Models;
using System;

namespace Smartplank.Scripts.Games.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private DamagingItem[] damagingItems;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private Vector2 direction = Vector3.down;
        private DataRepository _dataRepository;
        private GamesSharedModel _gamesModel;

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
            if (damagingItems.Length == 0)
                yield break;

            while (true)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(_gamesModel.EnemySpawnOptions.MinSpawnPause,
                    _gamesModel.EnemySpawnOptions.MaxSpawnPause));
                Spawn();
            }
        }

        private void Spawn()
        {
            var enemy = damagingItems[UnityEngine.Random.Range(0, damagingItems.Length)];
            var spawn = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            var enemyNew = Instantiate(enemy, spawn);

            if (enemyNew.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(direction * UnityEngine.Random.Range(_gamesModel.EnemySpawnOptions.MinSpeed,
                    _gamesModel.EnemySpawnOptions.MaxSpeed), ForceMode.Impulse);
            }
        }


    }
}
