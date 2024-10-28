using UnityEngine;
using Utils.CustomLogs;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Water")]
    public class WaterAbility : BaseAbility
    {
        [Header("Ability Settings")]
        [SerializeField] private WaterAbilityAttack _waveAbilityAttackPrefab;
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        
        public override AbilityType Type => AbilityType.Water;
        
        private PlayerMovement _playerMovement;
        
        public override void Initialize(GameObject target)
        {
            if (_playerMovement == null)
                _playerMovement = target.GetComponent<PlayerMovement>();
        }
        
        public override bool PerformAbility(GameObject target)
        {
            if (!_playerMovement.IsGrounded) return false;
            
            LogManager.Log("Water ability", FeatureType.Abilities);
            WaterAbilityAttack abilityAttack = Instantiate(_waveAbilityAttackPrefab, target.transform.position, target.transform.rotation);
            abilityAttack.Damage = _damage;
            abilityAttack.Duration = AbilityDuration;
            abilityAttack.Speed = _speed;

            return true;
        }
        
        public override bool PerformUltimate(GameObject target)
        {
            return false;
        }
    }
}