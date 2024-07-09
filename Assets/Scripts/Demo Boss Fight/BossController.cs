using System;
using System.Collections.Generic;
using GameEvents;
using Health;
using PlayerController.Abilities;
using PlayerController.Data;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Ability Settings")]
    [SerializeField] private AbilityTypeEvent _abilityUnlockEvent;
    [SerializeField] private AbilityType _abilityToUnlock;
    [SerializeField] private PlayerAbilitiesData _abilitiesData;

    [Header("Claws")]
    [SerializeField] private List<BossClaw> _claws;
    [SerializeField] private List<EntityHealth> _clawsHealth;
    
    [Space(10)]
    [SerializeField] private Color _deactiveColor;

    private int _clawIndex;
    
    public event Action OnFightFinished;

    private EntityHealth _headHealth;
    [SerializeField, ReadOnly] private int _remainingParts;

    private Transform _player;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        _headHealth = GetComponent<EntityHealth>();
        _remainingParts = 3;

        foreach (var claw in _claws)
        {
            claw.controller = this;
        }
    }

    private void Start()
    {
        _headHealth.AddListenerDeathEvent(Deactivate);
        foreach (var health in _clawsHealth)
        {
            health.AddListenerDeathEvent(CheckRemainingParts);
        }
    }

    public void StartFight(Transform player)
    {
        _clawIndex = 0;
        _player = player;
        _claws[_clawIndex].Attack(_player.position);
    }

    private void FinishFight()
    {
        OnFightFinished?.Invoke();
        
        if (!_abilitiesData.IsAbilityUnlock(_abilityToUnlock))
        {
            _abilitiesData.UnlockAbility(_abilityToUnlock);
            if (_abilityUnlockEvent)
                _abilityUnlockEvent.Raise(_abilityToUnlock);
        }
        
        Destroy(gameObject);
    }

    private void CheckRemainingParts()
    {
        _remainingParts--;
        if (_remainingParts <= 0)
        {
            FinishFight();
        }
    }

    public void NextAttack()
    {
        _clawIndex = (_clawIndex + 1) % _claws.Count;
        BossClaw claw = _claws[_clawIndex];
        if (claw.IsActive)
        {
            claw.Attack(_player.position);
        }
        else
        {
            _clawIndex = (_clawIndex + 1) % _claws.Count;
            claw = _claws[_clawIndex];
            if (claw.IsActive)
            {
                claw.Attack(_player.position);
            }
        }
    }

    private void Deactivate()
    {
        CheckRemainingParts();
        _spriteRenderer.color = _deactiveColor;
    }
}