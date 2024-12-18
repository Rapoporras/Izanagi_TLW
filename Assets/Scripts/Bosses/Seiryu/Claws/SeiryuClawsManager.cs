using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bosses
{
    public class SeiryuClawsManager : MonoBehaviour
    {
        [Header("Claws")]
        [SerializeField] private SeiryuClaw _leftClaw;
        [SerializeField] private SeiryuClaw _rightClaw;

        public event Action OnReadyForAttack;

        private void OnEnable()
        {
            _leftClaw.OnClawReady += OnClawReady;
            _rightClaw.OnClawReady += OnClawReady;
        }
        
        private void OnDisable()
        {
            _leftClaw.OnClawReady -= OnClawReady;
            _rightClaw.OnClawReady -= OnClawReady;
        }

        private void OnClawReady()
        {
            OnReadyForAttack?.Invoke();
        }

        public void Attack(Vector3 playerPos, int phase, params float[] probabilities)
        {
            if (probabilities.Length < 2) return;
            
            SeiryuClaw nearestClaw = GetNearestClaw(playerPos);
            float attackSelection = Random.Range(0f, 1f);

            if (attackSelection < probabilities[0])
            {
                nearestClaw.FistPunch();
            }
            else if (attackSelection < probabilities[0] + probabilities[1])
            {
                nearestClaw.SweepingAttack();
            }
            else if (phase == 2)
            {
                // water attack
            }
        }

        public void TransitionAttack()
        {
            
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