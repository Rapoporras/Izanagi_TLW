using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        public override void Initialize(GameObject target)
        {
            if (_playerMovement == null)
                _playerMovement = target.GetComponent<PlayerMovement>();
            
            if (_playerAbilities == null)
                _playerAbilities = target.GetComponent<PlayerAbilities>();
        }
        
        public override bool PerformAbility(GameObject target)
        {
            if (!_playerMovement.IsGrounded) return false;
            
            Debug.Log("Fire ability");
            FireAbilityAttack abilityAttack =
                Instantiate(_fireAbilityAttackPrefab, target.transform.position, target.transform.rotation);
            abilityAttack.Damage = _abilityDamage;
            abilityAttack.Duration = AbilityDuration;
            
            _playerAbilities.StartCoroutine(HandlePlayerInput());

            return true;
        }
        
        public override bool PerformUltimate(GameObject target)
        {
            Debug.Log("Fire Ultimate");
            FireUltimateAttack ultimateAttack =
                Instantiate(_fireUltimateAttackPrefab, Vector3.zero, Quaternion.identity);
            ultimateAttack.damage = _ultimateDamage;
            ultimateAttack.duration = UltimateDuration;
            
            // no necesita desabilitar el input del jugador
            // la ulti setea el tiemScale a 0
            
            return true;
        }

        private IEnumerator HandlePlayerInput()
        {
            // TODO: freeze player in current position in air
            
            InputManager.Instance.PlayerActions.Disable();
            yield return new WaitForSeconds(AbilityDuration);
            InputManager.Instance.PlayerActions.Enable();
        }
    }
}