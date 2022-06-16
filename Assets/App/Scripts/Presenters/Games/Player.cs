using System;
using System.Collections;
using System.Collections.Generic;
using Smartplank.Scripts.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Smartplank.Scripts.Games
{
    public class Player : MonoBehaviour, IHealth
    {
        private int _health;
        public event Action onDeath;
        public event Action onDamageRecieved;
        private bool isDead;

        private void Start()
        {
            SetHealth(1);
        }

        public void SetHealth(int health)
        {
            _health = health;
        }

        public void GetDamage(int damage)
        {
            onDamageRecieved?.Invoke();
            if (_health - damage <= 0)
            {
                _health = 0;
                PlayerDie();
            }
            else
                _health -= damage;
        }

        private void PlayerDie()
        {
            if (isDead)
                return;
            isDead = true;
            onDeath?.Invoke();
            Invoke(nameof(GoMenu), 1.5f);
        }

        private void GoMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
