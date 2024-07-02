using UnityEngine;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Water")]
    public class WaterAbility : BaseAbility
    {
        [SerializeField] private WaterAttack _waveAttackPrefab;
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        
        public override AbilityType Type => AbilityType.Water;
        
        private PlayerMovement _playerMovement;
        
        public override bool PerformAction(GameObject target)
        {
            if (!_playerMovement.IsGrounded) return false;
            
            Debug.Log("Water ability");
            WaterAttack attack = Instantiate(_waveAttackPrefab, target.transform.position, target.transform.rotation);
            attack.Damage = _damage;
            attack.Duration = AbilityDuration;
            attack.Speed = _speed;

            return true;
        }

        public override void Initialize(GameObject target)
        {
            if (_playerMovement == null)
                _playerMovement = target.GetComponent<PlayerMovement>();
        }
    }
}