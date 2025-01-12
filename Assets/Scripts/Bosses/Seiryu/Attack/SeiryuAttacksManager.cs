using System;
using CameraSystem;
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
        
        [Header("Screen Shake")]
        [SerializeField] private ScreenShakeProfile _transitionAttackShake;
        [SerializeField] private ScreenShakeProfile _fistAttackShake;
        [Space(5)]
        [SerializeField] private ScreenShakeSource _screenShakeSource;
        
        [Header("Events")]
        [SerializeField] private VoidEvent _seiryuStalactitesEvent;

        private AudioSource _audioSource;

        public event Action OnReadyForAttack;

        private bool _transitionAttack;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Initialize()
        {
            _leftClaw.EnableDamage(true);
            _rightClaw.EnableDamage(true);
        }
        
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
                        
                        _audioSource.Play();
                        _screenShakeSource.TriggerScreenShake(_transitionAttackShake);
                        ControllerVibration.Instance.TriggerInstantVibration(0.7f, 0.3f, 0.5f);
                    }
                    else if (info.type == AttackType.Fist)
                    {
                        _screenShakeSource.TriggerScreenShake(_fistAttackShake);
                        ControllerVibration.Instance.TriggerInstantVibration(0.7f, 0.3f, 0.5f);
                    }
                    break;
            }
        }

        public void Attack(Vector3 playerPos, float fistProb, float sweepProb)
        {
            SeiryuClaw nearestClaw = GetNearestClaw(playerPos);
            float attackSelection = Random.Range(0f, 1f);

            if (attackSelection < fistProb)
            {
                nearestClaw.FistPunch(playerPos);
            }
            else if (attackSelection < fistProb + sweepProb)
            {
                nearestClaw.SweepingAttack();
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