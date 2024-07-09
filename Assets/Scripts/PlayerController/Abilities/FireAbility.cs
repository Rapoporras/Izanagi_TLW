using System.Collections;
using UnityEngine;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Fire")]
    public class FireAbility : BaseAbility
    {
        [SerializeField] private FireAttack _fireAttackPrefab;
        [SerializeField] private int _damage;
        
        public override AbilityType Type => AbilityType.Fire;

        private PlayerMovement _playerMovement;
        private PlayerAbilities _playerAbilities;
        
        public override bool PerformAction(GameObject target)
        {
            if (!_playerMovement.IsGrounded) return false;
            
            Debug.Log("Fire ability");
            FireAttack attack = Instantiate(_fireAttackPrefab, target.transform.position, target.transform.rotation);
            attack.Damage = _damage;
            attack.Duration = AbilityDuration;
            
            _playerAbilities.StartCoroutine(HandlePlayerInput());

            return true;
        }

        public override void Initialize(GameObject target)
        {
            if (_playerMovement == null)
                _playerMovement = target.GetComponent<PlayerMovement>();
            
            if (_playerAbilities == null)
                _playerAbilities = target.GetComponent<PlayerAbilities>();
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