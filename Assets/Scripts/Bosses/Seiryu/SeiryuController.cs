using System;
using Health;
using StateMachine;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bosses
{
    public class SeiryuController : BaseStateMachine<SeiryuState>
    {
        [Header("Dependencies")]
        [SerializeField] private SeiryuAttacksManager _attacksManager;
        [SerializeField] private SeiryuHead _seiryuHead;
        [Space(5)]
        [SerializeField] private TextMeshProUGUI _stateText;
        
        [Header("Settings")]
        [Tooltip("Tiempo de espera para volver a realizar un nuevo ataque")]
        [SerializeField] private Vector2 _attackWaitingRange = new Vector2(1.5f, 3f);
        [Tooltip("Porcentaje de vida que debe tener el boss para cambiar de fase")]
        [SerializeField, Range(0,1)] private float _changePhasePercentage = 0.5f;
        
        [Header("Phase 1")]
        [SerializeField, Range(0, 1)] private float _fistAttackProbPhase1 = 0.5f;
        [SerializeField, Range(0, 1)] private float _sweepingAttackProbPhase1 = 0.5f;
        
        [Header("Phase 2")]
        [SerializeField, Range(0, 1)] private float _fistAttackProbPhase2 = 0.4f;
        [SerializeField, Range(0, 1)] private float _sweepingAttackProbPhase2 = 0.6f;
        
        public bool CanStartFight { get; private set; }
        public bool WaitForNextAttack { get; private set; }
        
        [HideInInspector] public bool transitionToNextPhase;
        [HideInInspector] public int phase;
        
        public event Action OnFightFinished;

        private Transform _player;
        private EntityHealth _health;

        private SpriteRenderer[] _seiryuSprites;
        
        private void Awake()
        {
            _health = GetComponent<EntityHealth>();
            _seiryuSprites = GetComponentsInChildren<SpriteRenderer>();
        }

        protected override void Start()
        {
            base.Start();
            SetSpritesAlpha(0);
        }

        protected override void Update()
        {
            base.Update();
            _stateText.text = $"State: {_currentState.StateKey.ToString()}";
        }

        private void OnEnable()
        {
            _health.AddListenerOnHit(UpdatePhase);
            _attacksManager.OnReadyForAttack += OnReadyForAttack;
        }

        private void OnDisable()
        {
            _health.RemoveListenerOnHit(UpdatePhase);
            _attacksManager.OnReadyForAttack -= OnReadyForAttack;
        }

        protected override void SetStates()
        {
            States.Add(SeiryuState.Init, new SeiryuInitCombatState(SeiryuState.Init, this));
            States.Add(SeiryuState.Combat, new SeiryuCombatState(SeiryuState.Combat, this));
            States.Add(SeiryuState.Waiting, new SeiryuWaitingState(SeiryuState.Waiting, this));
            States.Add(SeiryuState.Transition, new SeiryuTransitionState(SeiryuState.Transition, this));
            States.Add(SeiryuState.Dead, new SeiryuDeadState(SeiryuState.Dead, this));

            _currentState = States[SeiryuState.Init];
        }

        public void StartFight(Transform player)
        {
            _player = player;
            CanStartFight = true;
            phase = 1;
            
            _seiryuHead.Initialize(_player);
        }

        public float GetAttackWaitingTime()
        {
            return Random.Range(_attackWaitingRange.x, _attackWaitingRange.y);
        }

        public void TryToAttack()
        {
            if (phase == 1)
            {
                _attacksManager.Attack(_player.position, phase, _fistAttackProbPhase1, _sweepingAttackProbPhase1);
            }
            else if (phase == 2)
            {
                _attacksManager.Attack(_player.position, phase, 
                    _fistAttackProbPhase2, _sweepingAttackProbPhase2);
            }
            WaitForNextAttack = false;
        }

        public void TransitionAttack()
        {
            _attacksManager.TransitionAttack();
            WaitForNextAttack = false;
        }

        public void SetSpritesAlpha(float alpha)
        {
            foreach (var spriteRenderer in _seiryuSprites)
            {
                Color color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }
        }

        public void FinishBattle()
        {
            Destroy(gameObject);
        }
        
        private void OnReadyForAttack()
        {
            WaitForNextAttack = true;
        }

        private void UpdatePhase()
        {
            if (phase < 2 && HealthPercentage() <= _changePhasePercentage)
            {
                phase = 2;
                transitionToNextPhase = true;
            }
            else if (phase < 3 && HealthPercentage() == 0)
            {
                phase = 3;
                transitionToNextPhase = true;
            }
        }

        private float HealthPercentage()
        {
            return _health.CurrentHealth * 1f / _health.maxHealth;
        }
    }
}