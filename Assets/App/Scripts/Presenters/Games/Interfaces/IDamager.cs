using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.Games
{
    public interface IDamager 
    {
        public void DealDamage(IHealth healthOwner);
    }
}
