using Health;
using UnityEngine;
using Utils.CustomLogs;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Ground")]
    public class GroundAbility : BaseAbility
    {
        public override AbilityType Type => AbilityType.Ground;

        private PlayerHealth _playerHealth;
        private PlayerAbilities _playerAbilities;
        
        public override void Initialize(GameObject target)
        {
            if (_playerHealth == null)
                _playerHealth = target.GetComponent<PlayerHealth>();
            
            if (_playerAbilities == null)
                _playerAbilities = target.GetComponent<PlayerAbilities>();
            
            _playerHealth.OnShieldLost += HideShield;
        }
        
        public override bool PerformAbility(GameObject target)
        {
            if (_playerHealth.HasShield) return false;
            
            LogManager.Log("Ground ability", FeatureType.Abilities);
            _playerHealth.HasShield = true;
            _playerAbilities.ShowShield(true);

            return true;
        }

        
        public override bool PerformUltimate(GameObject target)
        {
            return false;
        }

        private void HideShield()
        {
            LogManager.Log("hide shield", FeatureType.Abilities);
            _playerAbilities.ShowShield(false);
        }
    }
}