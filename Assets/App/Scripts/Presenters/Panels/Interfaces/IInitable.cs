using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.Presenters.Panels
{
    public interface IInitable<T>
    {
        public void Init(T initData);
    }
}
