using System.Collections;
using UnityEngine;
using Utils.CustomLogs;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Air")]
    public class AirAbility : BaseAbility
    {
        [Header("Ability Settings")]
        [SerializeField, Range(1f, 2f)] private float _damageMultiplier;
        
        public override AbilityType Type => AbilityType.Air;

        private PlayerAttack _playerAttack;
        private PlayerAbilities _playerAbilities;

        private bool _boostActive;
        
        public override void Initialize(GameObject target)
        {
            if (_playerAttack == null)
                _playerAttack = target.GetComponent<PlayerAttack>();
            
            if (_playerAbilities == null)
                _playerAbilities = target.GetComponent<PlayerAbilities>();
        }
        
        public override bool PerformAbility(GameObject target)
        {
            if (_boostActive) return false;
            
            LogManager.Log("Air ability", FeatureType.Abilities);
            _playerAttack.AttackAirBoost = _damageMultiplier;
            _playerAttack.StartCoroutine(HandlePlayerBoost(target));

            return true;
        }

        public override bool PerformUltimate(GameObject target)
        {
            return false;
        }

        private IEnumerator HandlePlayerBoost(GameObject target)
        {
            _boostActive = true;
            
            _playerAbilities.ShowBoostIcon(true);
            yield return new WaitForSeconds(AbilityDuration);
            _playerAttack.AttackAirBoost = 1f;
            _playerAbilities.ShowBoostIcon(false);

            _boostActive = false;
        }
    }
}