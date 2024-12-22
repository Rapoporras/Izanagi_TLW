using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Bosses
{
    public class SeiryuClaw : MonoBehaviour
    {
        private enum State
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
        
        public event Action<ClawInfo> OnStateChange;

        private Coroutine _attackCoroutine;
        [SerializeField, ReadOnly] private State _state;
        private Rigidbody2D _rb2d;

        private Vector3 _initialPosition;

        private Timer _recoveringTimer;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _state = State.Waiting;
            _initialPosition = transform.position;

            _recoveringTimer = new Timer(_timeBeforeRecovering);
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Attacking:
                    ResolveAttackState();
                    break;
                case State.Recovering:
                    ResolveRecoveringState();
                    break;
                case State.Waiting:
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
                _state = State.Waiting;
                _recoveringTimer.Reset();
                TriggerStateChangeEvent(ClawState.Waiting);
            }
        }

        private void ResolveWaitingState() { }
        #endregion

        #region FIST ATTACK
        [ContextMenu("Fist Attack")]
        public void FistPunch()
        {
            InitAttack(_FistPunch());
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
            _state = State.Recovering;
        }
        #endregion
        
        #region SWEEPING ATTACK
        [ContextMenu("Sweeping Attack")]
        public void SweepingAttack()
        {
            InitAttack(_SweepingAttack());
        }

        private IEnumerator _SweepingAttack()
        {
            float sweepLength = MathUtils.ApproxBezierCurveLength(100, _initialPosition, _sweepStartPosition.position,
                _stageCenterPosition.position);
            float sweepDistanceTraveled = 0f;
            
            while (sweepDistanceTraveled < sweepLength)
            {
                sweepDistanceTraveled += _sweepingSpeed * Time.deltaTime;
                float sweepProgression = sweepDistanceTraveled / sweepLength;
                sweepProgression = Mathf.Clamp01(sweepProgression);

                Vector3 nextPos = MathUtils.BezierCurvePos(sweepProgression, _initialPosition,
                    _sweepStartPosition.position, _stageCenterPosition.position);
                _rb2d.MovePosition(nextPos);
                
                yield return null;
            }
            
            yield return new WaitForSeconds(_timeBeforeRecovering);
            _state = State.Recovering;
        }
        #endregion
        
        #region TRANSITION ATTACK

        public void TransitionAttack()
        {
            InitAttack(_TransitionAttack());
        }

        private IEnumerator _TransitionAttack()
        {
            bool isInPlace = false;
            while (!isInPlace)
            {
                isInPlace = MoveToTarget(_stageCenterPosition.position, _fistAttackSpeed * Time.deltaTime);
                yield return null;
            }
            
            TriggerStateChangeEvent(ClawState.FinishAttack);
            
            yield return new WaitForSeconds(_timeBeforeRecovering);
            _state = State.Recovering;
        }
        #endregion
        
        #region UTILS
        private void InitAttack(IEnumerator attackCoroutine)
        {
            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            _state = State.Attacking;
            _attackCoroutine = StartCoroutine(attackCoroutine);
        }

        private void TriggerStateChangeEvent(ClawState state)
        {
            ClawInfo info = new ClawInfo { state = state };
            OnStateChange?.Invoke(info);
        }
        
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Vector3 initPos = transform.position;
            if (Application.isPlaying)
                initPos = _initialPosition;

            Vector3 from = initPos;
            for (int i = 1; i <= 100; i++)
            {
                Vector3 to = MathUtils.BezierCurvePos(i / 100f, initPos, _sweepStartPosition.position,
                    _stageCenterPosition.position);
                Gizmos.DrawLine(from, to);
                from = to;
            }
        }
    }
}