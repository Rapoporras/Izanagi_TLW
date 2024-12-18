using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Bosses
{
    public class SeiryuClaw : MonoBehaviour
    {
        enum ClawState
        {
            Attacking, Recovering, Waiting
        }

        [Header("General")]
        [SerializeField] private float _timeBeforeRecovering;
        [SerializeField] private float _recoveringSpeed;
        
        [Header("Positions")]
        [SerializeField] private Transform _fistHitPosition;
        [SerializeField] private Transform _sweepStartPosition;
        [SerializeField] private Transform _stageCenterPosition;
        
        [Header("Fist Attack")]
        [SerializeField] private float _fistAttackSpeed;

        [Header("Sweeping Attack")]
        [SerializeField] private float _sweepingSpeed;
        [SerializeField] private float _sweepingWaitingTime;
        
        public event Action OnClawReady;

        private Coroutine _attackCoroutine;
        [SerializeField, ReadOnly] private ClawState _state;

        private Vector3 _initialPosition;

        private Timer _recoveringTimer;

        private void Start()
        {
            _state = ClawState.Waiting;
            _initialPosition = transform.position;

            _recoveringTimer = new Timer(_timeBeforeRecovering);
        }

        private void Update()
        {
            switch (_state)
            {
                case ClawState.Attacking:
                    ResolveAttackState();
                    break;
                case ClawState.Recovering:
                    ResolveRecoveringState();
                    break;
                case ClawState.Waiting:
                    ResolveWaitingState();
                    break;
            }
        }
        
        #region STATES
        private void ResolveAttackState() { }

        private void ResolveRecoveringState()
        {
            if (!_recoveringTimer.Finished)
            {
                _recoveringTimer.Tick(Time.deltaTime);
                return;
            }
            
            bool isInPlace = MoveToTarget(_initialPosition, _recoveringSpeed * Time.deltaTime);
            if (isInPlace)
            {
                _state = ClawState.Waiting;
                _recoveringTimer.Reset();
                OnClawReady?.Invoke();
            }
        }

        private void ResolveWaitingState() { }
        #endregion

        #region FIST ATTACK
        [ContextMenu("Fist Attack")]
        public void FistPunch()
        {
            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            _state = ClawState.Attacking;
            _attackCoroutine = StartCoroutine(_FistPunch());
        }

        private IEnumerator _FistPunch()
        {
            bool isInPlace = false;
            while (!isInPlace)
            {
                isInPlace = MoveToTarget(_fistHitPosition.position, _fistAttackSpeed * Time.deltaTime);
                yield return null;
            }
            
            yield return new WaitForSeconds(_timeBeforeRecovering);
            _state = ClawState.Recovering;
        }
        #endregion
        
        #region SWEEPING ATTACK
        [ContextMenu("Sweeping Attack")]
        public void SweepingAttack()
        {
            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            _state = ClawState.Attacking;
            _attackCoroutine = StartCoroutine(_SweepingAttack());
        }

        private IEnumerator _SweepingAttack()
        {
            bool placedInCorner = false;
            while (!placedInCorner)
            {
                placedInCorner = MoveToTarget(_sweepStartPosition.position, _sweepingSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(_sweepingWaitingTime);
            
            bool placedInStageCenter = false;
            while (!placedInStageCenter)
            {
                placedInStageCenter = MoveToTarget(_stageCenterPosition.position, _sweepingSpeed * Time.deltaTime);
                yield return null;
            }
            
            yield return new WaitForSeconds(_timeBeforeRecovering);
            _state = ClawState.Recovering;
        }
        #endregion
        
        #region UTILS
        /// <summary>
        /// Move transform to a target position.
        /// </summary>
        /// <param name="targetPos">Target position</param>
        /// <param name="delta">Max distance delta</param>
        /// <returns>If the transform has reached the target position.</returns>
        private bool MoveToTarget(Vector3 targetPos, float delta)
        {
            Vector3 currentPos = transform.position;
            if (currentPos == targetPos)
                return true;
            
            transform.position = Vector3.MoveTowards(currentPos, targetPos, delta);
            return false;
        }
        #endregion
    }
}