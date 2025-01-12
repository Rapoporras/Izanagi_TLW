using System.Collections;
using GameEvents;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Bosses
{
    public class SeiryuClaw : MonoBehaviour
    {
        public enum State
        {
            Attacking, Recovering, Waiting
        }

        [Header("General")]
        [SerializeField] private ClawSide _side;
        [Tooltip("Tiempo de espera para volver a su posición inicial después de realizar un ataque")]
        [SerializeField] private float _timeBeforeRecovering;
        [Tooltip("Velocidad con la que vuelve a la posición inicial")]
        [SerializeField] private float _recoveringSpeed;

        [Header("Positions")]
        [SerializeField] private Transform _transitionAttackPos;
        [Space(5)]
        [SerializeField] private Transform _sweepCurveControlPos;
        [SerializeField] private Transform _sweepEndPos;

        [Header("Rest movement")]
        [SerializeField] private float _restRadiusArea;
        [SerializeField] private float _restMovementSpeed;
        
        [Header("Fist Attack")]
        [SerializeField] private float _anticipationHeight;
        [SerializeField] private float _anticipationTime;
        [SerializeField] private float _fistAttackSpeed;

        [Header("Sweeping Attack")]
        [SerializeField] private float _sweepingSpeed;

        [Header("Events")]
        [SerializeField] private SeiryuAttackInfoEvent _attackEvent;
        
        [FormerlySerializedAs("punchSound")]
        [Header("Audio")]
        [SerializeField] private AudioClip fistSound;
        [SerializeField] private AudioClip sweepSound;

        private AudioSource _audioSource;
        private bool _didSweepSoundPlay;
        
        private Rigidbody2D _rb2d;
        private Animator _animator;
        private ContactDamage _contactDamage;

        private int _defaultAnimHash;
        private int _fistAnimHash;
        private int _sweepAnimHash;
        
        private Coroutine _attackCoroutine;
        private State _state;
        public State state => _state;

        private Vector3 _initialPosition;
        private Vector3 _restTargetPos;
        
        private Timer _recoveringTimer;
        
        

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _contactDamage = GetComponent<ContactDamage>();
            _contactDamage.isActive = false;

            _audioSource = GetComponent<AudioSource>();

            _defaultAnimHash = Animator.StringToHash("default");
            _fistAnimHash = Animator.StringToHash("fist");
            _sweepAnimHash = Animator.StringToHash("sweep");
        }

        private void Start()
        {
            _state = State.Waiting;
            _initialPosition = transform.position;
            _restTargetPos = _initialPosition;

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
                _restTargetPos = _initialPosition;
                
                _animator.SetTrigger(_defaultAnimHash);
                TriggerStateChangeEvent(AttackState.Waiting, AttackType.None, _side);
            }
        }

        private void ResolveWaitingState()
        {
            bool isInPlace = MoveToTarget(_restTargetPos, _restMovementSpeed * Time.deltaTime);
            if (isInPlace)
            {
                float angle = Random.Range(0, Mathf.PI * 2);
                float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * _restRadiusArea;

                float x = _initialPosition.x + distance * Mathf.Cos(angle);
                float y = _initialPosition.y + distance * Mathf.Sin(angle);

                _restTargetPos = new Vector3(x, y, transform.position.z);
            }
        }
        #endregion

        #region FIST ATTACK
        [ContextMenu("Fist Attack")]
        public void FistPunch(Vector3 targetPos)
        {
            InitAttack(_FistPunch(targetPos), AttackType.Fist);
        }

        private IEnumerator _FistPunch(Vector3 targetPos)
        {
            bool isInPlace = false;
            
            Vector3 anticipationPos = transform.position;
            anticipationPos.y += _anticipationHeight;
            while (!isInPlace)
            {
                isInPlace = MoveToTarget(anticipationPos, _fistAttackSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(_anticipationTime);
            
            isInPlace = false;
            targetPos.y = _transitionAttackPos.position.y;
            while (!isInPlace)
            {
                // set targetPos.y near the floor
                isInPlace = MoveToTarget(targetPos, _fistAttackSpeed * Time.deltaTime);
                yield return null;
            }

            _audioSource.volume = 0.65f;
            _audioSource.clip = fistSound;
            _audioSource.Play();
            
            TriggerStateChangeEvent(AttackState.FinishAttack, AttackType.Fist, _side);
            
            yield return new WaitForSeconds(_timeBeforeRecovering);
            _state = State.Recovering;
        }
        #endregion
        
        #region SWEEPING ATTACK
        [ContextMenu("Sweeping Attack")]
        public void SweepingAttack()
        {
            InitAttack(_SweepingAttack(), AttackType.Sweep);
        }

        private IEnumerator _SweepingAttack()
        {
            float sweepLength = MathUtils.ApproxBezierCurveLength(100, _initialPosition, _sweepCurveControlPos.position,
                _sweepEndPos.position);
            float sweepDistanceTraveled = 0f;
            
            while (sweepDistanceTraveled < sweepLength)
            {
                sweepDistanceTraveled += _sweepingSpeed * Time.deltaTime;
                float sweepProgression = sweepDistanceTraveled / sweepLength;
                sweepProgression = Mathf.Clamp01(sweepProgression);

                if (sweepProgression > 0.5 && !_didSweepSoundPlay)
                {
                    _audioSource.clip = sweepSound;
                    _audioSource.volume = 1f;
                    _audioSource.Play();
                    _didSweepSoundPlay = true;
                }

                Vector3 nextPos = MathUtils.BezierCurvePos(sweepProgression, _initialPosition,
                    _sweepCurveControlPos.position, _sweepEndPos.position);
                _rb2d.MovePosition(nextPos);
                
                yield return null;
            }

            _didSweepSoundPlay = false;
            
            TriggerStateChangeEvent(AttackState.FinishAttack, AttackType.Sweep, _side);
            
            yield return new WaitForSeconds(_timeBeforeRecovering);
            _state = State.Recovering;
        }
        #endregion
        
        #region TRANSITION ATTACK
        public void TransitionAttack(bool triggerEvent)
        {
            InitAttack(_TransitionAttack(triggerEvent), AttackType.Transition);
        }

        private IEnumerator _TransitionAttack(bool triggerEvent)
        {
            bool isInPlace = false;
            
            Vector3 anticipationPos = transform.position;
            anticipationPos.y += _anticipationHeight;
            while (!isInPlace)
            {
                isInPlace = MoveToTarget(anticipationPos, _fistAttackSpeed * Time.deltaTime);
                yield return null;
            }
            
            yield return new WaitForSeconds(_anticipationTime);
            
            isInPlace = false;
            while (!isInPlace)
            {
                isInPlace = MoveToTarget(_transitionAttackPos.position, _fistAttackSpeed * Time.deltaTime);
                yield return null;
            }
            
            if (triggerEvent)
                TriggerStateChangeEvent(AttackState.FinishAttack, AttackType.Transition, _side);
            
            yield return new WaitForSeconds(_timeBeforeRecovering);
            _state = State.Recovering;
        }
        #endregion
        
        #region UTILS
        public void EnableDamage(bool isEnable)
        {
            _contactDamage.isActive = isEnable;
        }
        
        private void InitAttack(IEnumerator attackCoroutine, AttackType attackType)
        {
            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            _state = State.Attacking;
            _attackCoroutine = StartCoroutine(attackCoroutine);
            
            TriggerStateChangeEvent(AttackState.StartAttack, attackType, _side);
            
            // set animation
            switch (attackType)
            {
                case AttackType.Fist:
                    _animator.SetTrigger(_fistAnimHash);
                    break;
                case AttackType.Sweep:
                    _animator.SetTrigger(_sweepAnimHash);
                    break;
                case AttackType.Transition:
                    _animator.SetTrigger(_fistAnimHash);
                    break;
            }
        }

        private void TriggerStateChangeEvent(AttackState state, AttackType type, ClawSide side)
        {
            SeiryuAttackInfo info = new SeiryuAttackInfo { state = state, type = type, side = side};
            if (_attackEvent)
                _attackEvent.Raise(info);
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
                Vector3 to = MathUtils.BezierCurvePos(i / 100f, initPos, _sweepCurveControlPos.position,
                    _sweepEndPos.position);
                Gizmos.DrawLine(from, to);
                from = to;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(initPos, _restRadiusArea);
        }
    }
}