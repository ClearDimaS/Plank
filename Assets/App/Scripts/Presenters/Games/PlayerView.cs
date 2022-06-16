using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Smartplank.Scripts.Utils;
using UnityEngine;

namespace Smartplank.Scripts.Games
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private MovePresenter movePresenter;
        [SerializeField] private GameObject gfxGO;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Player player;

        [SerializeField] private float recieveDamageDuration;
        [SerializeField] private float walkSleepTime;
        [SerializeField] private bool animateDirection = true;
        [SerializeField] private bool hasAnimation = false;
        [SerializeField] private bool inverseAnimationDirection = false;

        private float lastTimeWalked;

        private void Awake()
        {
            movePresenter.onDirectionChanged += ChangeDirection;
            movePresenter.onPosChanged += Walk;
            player.onDamageRecieved += Damaged;
        }

        private void Update()
        {
            if(hasAnimation)
                if (Time.time - lastTimeWalked > walkSleepTime)
                animator.SetBool(Constants.WALK, false);
        }

        private void Walk(Vector3 obj)
        {
            lastTimeWalked = Time.time;
            if(hasAnimation)
            animator.SetBool(Constants.WALK, true);
        }

        private void Damaged()
        {
            spriteRenderer.DOColor(Color.red, recieveDamageDuration).OnComplete(() =>
            {
                spriteRenderer.DOColor(Color.white, recieveDamageDuration);
            });
        }

        private void ChangeDirection(float direction)
        {
            if(animateDirection)
                gfxGO.transform.localScale = new Vector3(inverseAnimationDirection ? -direction : direction, 1f, 1f);
        }
    }
}
