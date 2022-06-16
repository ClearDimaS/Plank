using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Smartplank.Scripts
{
    public class WorkoutRest : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeLeftText;

        public void SetTime(float time)
        {
            timeLeftText.text = ((int)time).ToString();
        }
    }
}
