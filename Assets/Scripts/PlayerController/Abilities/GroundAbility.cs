using Health;
using UnityEngine;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Ground")]
    public class GroundAbility : BaseAbility
    {
        public override AbilityType Type => AbilityType.Ground;

        private PlayerHealth _playerHealth;
        private PlayerAbilities _playerAbilities;
        
        public override bool PerformAction(GameObject target)
        {
            if (_playerHealth.HasShield) return false;
            
            Debug.Log("Ground ability");
            _playerHealth.HasShield = true;
            _playerAbilities.ShowShield(true);

            return true;
        }

        public override void Initialize(GameObject target)
        {
            if (_playerHealth == null)
                _playerHealth = target.GetComponent<PlayerHealth>();
            
            if (_playerAbilities == null)
                _playerAbilities = target.GetComponent<PlayerAbilities>();
            
            _playerHealth.OnShieldLost += HideShield;
        }

        private void HideShield()
        {
            Debug.Log("hide shield");
            _playerAbilities.ShowShield(false);
        }
    }
}