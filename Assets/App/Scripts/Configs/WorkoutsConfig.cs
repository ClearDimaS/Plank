using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.Configs
{
    [CreateAssetMenu(fileName = "WorkoutsConfig", menuName = "Configs/Workouts", order = 1)]
    public class WorkoutsConfig : ScriptableObject
    {
        [SerializeField] private int[] workOutsTimes;
        public int[] WorkOutsTimes => workOutsTimes;
    }
}
