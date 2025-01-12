using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.CustomLogs;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Fire")]
    public class FireAbility : BaseAbility
    {
        [Header("Ability Settings")]
        [SerializeField] private FireAbilityAttack _fireAbilityAttackPrefab;
        [SerializeField] private int _abilityDamage;

        [Header("Ultimate Settings")]
        [SerializeField] private FireUltimateAttack _fireUltimateAttackPrefab;
        [SerializeField] private int _ultimateDamage;
        
        public override AbilityType Type => AbilityType.Fire;

        private PlayerMovement _playerMovement;
        private PlayerAbilities _playerAbilities;

        private Transform _spawnPoint;
        
        public override void Initialize(GameObject target)
        {
            if (_playerMovement == null)
                _playerMovement = target.GetComponent<PlayerMovement>();
            
            if (_playerAbilities == null)
                _playerAbilities = target.GetComponent<PlayerAbilities>();

            _spawnPoint = _playerAbilities.fireAbilitySpawnPos;
        }
        
        public override bool PerformAbility(GameObject target)
        {
            if (!_playerMovement.IsGrounded) return false;
            
            LogManager.Log("Fire ability", FeatureType.Abilities);
            FireAbilityAttack abilityAttack =
                Instantiate(_fireAbilityAttackPrefab, _spawnPoint.position, target.transform.rotation);
            abilityAttack.Damage = _abilityDamage;
            abilityAttack.Duration = AbilityDuration;
            
            _playerAbilities.StartCoroutine(HandlePlayerInput());

            return true;
        }
        
        public override bool PerformUltimate(GameObject target)
        {
            LogManager.Log("Fire Ultimate", FeatureType.Abilities);
            FireUltimateAttack ultimateAttack =
                Instantiate(_fireUltimateAttackPrefab, Vector3.zero, Quaternion.identity);
            ultimateAttack.damage = _ultimateDamage;
            ultimateAttack.duration = UltimateDuration;
            
            return true;
        }

        private IEnumerator HandlePlayerInput()
        {
            // TODO: freeze player in current position in air
            
            InputManager.DisablePlayerActions();
            yield return new WaitForSeconds(AbilityDuration);
            InputManager.EnablePlayerActions();
        }
    }
}