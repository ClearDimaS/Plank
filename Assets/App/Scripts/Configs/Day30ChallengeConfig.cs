using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.Configs
{
    [CreateAssetMenu(fileName = "30DayChallengeConfig", menuName = "Configs/30DayChallenge", order = 0)]
    public class Day30ChallengeConfig : ScriptableObject
    {
        [SerializeField] private List<ChallengeDayItem> challengeDayItems;
        public List<ChallengeDayItem> ChallengeDayItems => challengeDayItems;

    }
}

[System.Serializable]
public class ChallengeDayItem
{
    public int[] ChallengeTimes;
}
