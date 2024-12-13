using System;
using System.Collections.Generic;
using Bosses.States;
using GameEvents;
using Health;
using PlayerController.Abilities;
using PlayerController.Data;
using StateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bosses
{
    public class SeiryuController : BaseStateMachine<SeiryuState>
    {
        [SerializeField] private Vector2 _attackWaitingRange = new Vector2(1.5f, 3f); 
        
        [Header("Phase 1")]
        [SerializeField, Range(0, 1)] private float _fistAttackProbFirstPhase = 0.5f;
        [SerializeField, Range(0, 1)] private float _sweepingAttackProbFirstPhase = 0.5f;
        
        [Header("Phase 2")]
        [SerializeField, Range(0,1)] private float _secondPhasePercentage = 0.66f;
        
        [Header("Phase 3")]
        [SerializeField, Range(0,1)] private float _thirdPhasePercentage = 0.33f;
        
        [Header("Ability Settings")]
        [SerializeField] private AbilityTypeEvent _abilityUnlockedEvent;
        [SerializeField] private AbilityType _abilityToUnlock;
        [SerializeField] private PlayerAbilitiesData _abilitiesData;

        [SerializeField] private List<SeiryuClaw> _claws;

        
        public bool CanStartFight { get; private set; }
        public int phase;
        
        public event Action OnFightFinished;

        private Transform _player;
        private EntityHealth _health;
        
        private void Awake()
        {
            _health = GetComponent<EntityHealth>();
        }

        private void OnEnable()
        {
            _health.AddListenerOnHit(UpdatePhase);
        }

        private void OnDisable()
        {
            _health.RemoveListenerOnHit(UpdatePhase);
        }

        protected override void SetStates()
        {
            States.Add(SeiryuState.Init, new SeiryuInitCombatState(SeiryuState.Init, this));
            States.Add(SeiryuState.Combat, new SeiryuCombatState(SeiryuState.Combat, this));
            States.Add(SeiryuState.Transition, new SeiryuTransitionState(SeiryuState.Transition, this));
            States.Add(SeiryuState.Dead, new SeiryuDeadState(SeiryuState.Dead, this));

            _currentState = States[SeiryuState.Init];
        }

        public void StartFight(Transform player)
        {
            _player = player;
            CanStartFight = true;
            phase = 1;
        }

        public float GetAttackWaitingTime()
        {
            return Random.Range(_attackWaitingRange.x, _attackWaitingRange.y);
        }

        private void UpdatePhase()
        {
            if (phase < 2 && HealthPercentage() <= _secondPhasePercentage)
            {
                phase = 2;
            }
            else if (phase < 3 && HealthPercentage() <= _thirdPhasePercentage)
            {
                phase = 3;
            }
        }

        private float HealthPercentage()
        {
            return _health.CurrentHealth * 1f / _health.maxHealth;
        }
    }
}