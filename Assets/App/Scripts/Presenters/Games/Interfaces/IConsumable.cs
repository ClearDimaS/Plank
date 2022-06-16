using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.Games
{
    public interface IConsumable
    {
        public void Consume(GameObject target);
    }
}
