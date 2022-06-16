using System;
using System.Collections;
using System.Collections.Generic;
using Smartplank.Scripts.UserInput;
using UnityEngine;

namespace Smartplank.Scripts.Games
{
    public class MovePresenter : MonoBehaviour
    {
        [SerializeField] private Rigidbody playerRB;
        [SerializeField] private Rigidbody2D playerRB2D;
        [SerializeField] private Transform leftBorderTransform;
        [SerializeField] private Transform rightBorderTransform;

        private float lastDirectionX = 1f;
        private float lastInput = 0.5f;

        public event Action<float> onDirectionChanged;
        public event Action<Vector3> onPosChanged;

        private void Start()
        {
            var pos = Vector3.Lerp(leftBorderTransform.position, rightBorderTransform.position, lastInput);
            Move(pos);
        }

        private void FixedUpdate()
        {
            float curInput = GyroInputManager.Instance.InputGyroNormalized;
            if (Mathf.Approximately(curInput, lastInput))
                return;

            var currentDirectionX = Mathf.Sign(curInput - lastInput);
            if (lastDirectionX != currentDirectionX)
            {
                onDirectionChanged?.Invoke(currentDirectionX);
                lastDirectionX = currentDirectionX;
            }

            var pos = Vector3.Lerp(leftBorderTransform.position, rightBorderTransform.position, (curInput + 1f) / 2f);
            Move(pos);
        }

        private void Move(Vector3 pos)
        {
            onPosChanged?.Invoke(pos);
            if (playerRB2D == null)
                playerRB.MovePosition(pos);
            else
                playerRB2D.MovePosition(new Vector2(pos.x, pos.y));

            lastInput = GyroInputManager.Instance.InputGyroNormalized;
        }
    }
}
