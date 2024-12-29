﻿using System;
using GameEvents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bosses
{
    public class SeiryuAttacksManager : MonoBehaviour, ISeiryuAttackStateHandler
    {
        [Header("Claws")]
        [SerializeField] private SeiryuClaw _leftClaw;
        [SerializeField] private SeiryuClaw _rightClaw;

        [Header("Events")]
        [SerializeField] private VoidEvent _seiryuStalactitesEvent;

        public event Action OnReadyForAttack;

        private bool _transitionAttack;
        
        public void OnAttackStateChange(SeiryuAttackInfo info)
        {
            switch (info.state)
            {
                case AttackState.Waiting:
                    OnReadyForAttack?.Invoke();
                    break;
                case AttackState.FinishAttack:
                    if (_transitionAttack)
                    {
                        _transitionAttack = false;
                        if (_seiryuStalactitesEvent)
                            _seiryuStalactitesEvent.Raise();
                    }
                    break;
            }
        }

        public void Attack(Vector3 playerPos, int phase, float fistProb, float sweepProb)
        {
            SeiryuClaw nearestClaw = GetNearestClaw(playerPos);
            float attackSelection = Random.Range(0f, 1f);

            if (attackSelection < fistProb)
            {
                nearestClaw.FistPunch();
            }
            else if (attackSelection < fistProb + sweepProb)
            {
                nearestClaw.SweepingAttack();
            }
            else if (phase == 2)
            {
                // water attack
                nearestClaw.FistPunch();
            }
        }

        public void TransitionAttack()
        {
            _leftClaw.TransitionAttack(true);
            _rightClaw.TransitionAttack(false);

            _transitionAttack = true;
        }

        private SeiryuClaw GetNearestClaw(Vector3 playerPos)
        {
            float leftDistance = Vector3.Distance(playerPos, _leftClaw.transform.position);
            float rightDistance = Vector3.Distance(playerPos, _rightClaw.transform.position);

            if (leftDistance > rightDistance)
                return _rightClaw;
            else
                return _leftClaw;
        }
    }
}