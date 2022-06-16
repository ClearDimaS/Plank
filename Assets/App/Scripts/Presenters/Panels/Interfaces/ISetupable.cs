using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.Presenters.Panels
{
    public interface ISetupable<T>
    {
        public void SetUp(T setupdData);
    }
}
