using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.Games
{
    public interface IHealth 
    {
        public event Action onDeath;

        public void SetHealth(int health);

        public void GetDamage(int damage);
    }
}
