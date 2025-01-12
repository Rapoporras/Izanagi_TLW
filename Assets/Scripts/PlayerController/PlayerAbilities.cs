using System.Collections;
using System.Collections.Generic;
using GlobalVariables;
using PlayerController.Abilities;
using PlayerController.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.CustomLogs;

namespace PlayerController
{
    public class PlayerAbilities : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerAbilitiesData _playerAbilitiesData;
        [SerializeField] private GameObject _boostIcon;
        [SerializeField] private GameObject _shield;
        [Space(10)]
        public Transform fireAbilitySpawnPos;
        
        [Header("Settings")]
        [SerializeField] private AbilityTypeReference _currentAbility;
        [SerializeField] private List<BaseAbility> _abilities;

        [Header("Cooldown")]
        [SerializeField] private FloatReference _cooldownProgress;

        [Header("Manna")]
        [SerializeField] private IntReference _currentMannaAmount;

        private float _cooldownTimer;
        private bool _isRechargingAbility;

        private Animator _animator;
        private int _abilityIndex;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            // set initial ability
            // TODO: check if current ability is available
            BaseAbility currentInfo = _abilities.Find(ability => ability.Type == _currentAbility);
            _animator.runtimeAnimatorController = currentInfo.Animator;
            _abilityIndex = _abilities.IndexOf(currentInfo);

            _cooldownProgress.Value = 1f;
            _isRechargingAbility = false;
            
            // initialize abilities
            foreach (var ability in _abilities)
            {
                ability.Initialize(gameObject);
            }

            AbilityType activeAbility = _playerAbilitiesData.GetFirstAbilityUnlocked();
            SetAbility(activeAbility);
        }

        private void OnEnable()
        {
            InputManager.PlayerActions.ChangeAbility.performed += OnChangeAbility;
            InputManager.PlayerActions.AbilityAction.performed += OnAbilityAction;
            InputManager.PlayerActions.Ultimate.performed += OnUltimateAction;
        }

        private void OnDisable()
        {
            InputManager.PlayerActions.ChangeAbility.performed -= OnChangeAbility;
            InputManager.PlayerActions.AbilityAction.performed -= OnAbilityAction;
            InputManager.PlayerActions.Ultimate.performed -= OnUltimateAction;
        }

        private void SetAbility(AbilityType type)
        {
            for (int i = 0; i < _abilities.Count; i++)
            {
                if (_abilities[i].Type == type)
                {
                    _abilityIndex = i;
                    _animator.runtimeAnimatorController = _abilities[i].Animator;
                    _currentAbility.Value = type;
                }
            }
        }

        private void OnChangeAbility(InputAction.CallbackContext context)
        {
            if (_isRechargingAbility) return;
            
            BaseAbility newAbility;
            do
            {
                _abilityIndex = (_abilityIndex + 1) % _abilities.Count;
                newAbility = _abilities[_abilityIndex];
                
            } while (!IsAbilityAvailable(newAbility.Type) || newAbility.Type == AbilityType.NoAbility);
            
            LogManager.Log($"Change to ability -> {newAbility.Type}", FeatureType.Abilities);
            _animator.runtimeAnimatorController = newAbility.Animator;
            _currentAbility.Value = newAbility.Type;
        }
        
        private void OnAbilityAction(InputAction.CallbackContext context)
        {
            if (_isRechargingAbility) return;
            
            BaseAbility ability = _abilities[_abilityIndex];

            if (ability.AbilityMannaCost <= _currentMannaAmount.Value)
            {
                if (ability.PerformAbility(gameObject))
                {
                    _currentMannaAmount.Value -= ability.AbilityMannaCost;
                    StartCoroutine(RechargeAbility(ability.CooldownDuration));
                }
            }
            else
            {
                LogManager.Log($"[NOT ENOUGH MANNA] Ability cost: {ability.AbilityMannaCost} - Manna amount: {_currentMannaAmount.Value}",
                    FeatureType.Abilities);
            }
        }
        
        private void OnUltimateAction(InputAction.CallbackContext context)
        {
            if (_isRechargingAbility) return;
            
            BaseAbility ability = _abilities[_abilityIndex];
            
            if (ability.UltimateMannaCost <= _currentMannaAmount.Value)
            {
                if (ability.PerformUltimate(gameObject))
                {
                    _currentMannaAmount.Value -= ability.UltimateMannaCost;
                    StartCoroutine(RechargeAbility(ability.CooldownDuration));
                }
            }
            else
            {
                LogManager.Log($"[NOT ENOUGH MANNA] Ultimate cost: {ability.UltimateMannaCost} - Manna amount: {_currentMannaAmount.Value}",
                    FeatureType.Abilities);
            }
        }

        private IEnumerator RechargeAbility(float duration)
        {
            _cooldownTimer = duration;
            _isRechargingAbility = true;

            while (_isRechargingAbility)
            {
                _cooldownTimer = Mathf.Max(0, _cooldownTimer - Time.deltaTime);
                _cooldownProgress.Value = _cooldownTimer / duration;
                if (_cooldownTimer <= 0)
                {
                    _isRechargingAbility = false;
                }
                
                yield return null;
            }
            
            _cooldownProgress.Value = 1f;
        }

        private bool IsAbilityAvailable(AbilityType type)
        {
            switch (type)
            {
                case AbilityType.NoAbility:
                    return true;
                case AbilityType.Air:
                    return _playerAbilitiesData.airAbility;
                case AbilityType.Fire:
                    return _playerAbilitiesData.fireAbility;
                case AbilityType.Ground:
                    return _playerAbilitiesData.groundAbility;
                case AbilityType.Water:
                    return _playerAbilitiesData.waterAbility;
            }
            
            return false;
        }

        public void ShowBoostIcon(bool activate)
        {
            _boostIcon.SetActive(activate);
        }

        public void ShowShield(bool activate)
        {
            _shield.SetActive(activate);
        }
    }
}