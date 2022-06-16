using System.Collections;
using System.Collections.Generic;
using Smartplank.Scripts.Configs.Games;
using UnityEngine;

namespace Smartplank.Scripts.Games
{
    public class DamagingItem : MonoBehaviour, IDamager
    {
        [SerializeField] private DamagingItemObject damagingItemObject;

        public void DealDamage(IHealth healthOwner)
        {
            healthOwner.GetDamage(damagingItemObject.Damage);
        }
    }
}
