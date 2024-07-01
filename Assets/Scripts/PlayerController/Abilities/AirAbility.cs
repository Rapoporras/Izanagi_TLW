using System.Collections;
using UnityEngine;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Air")]
    public class AirAbility : BaseAbility
    {
        [SerializeField, Range(1f, 2f)] private float _damageMultiplier;
        
        public override AbilityType Type => AbilityType.Air;

        private PlayerAttack _playerAttack;
        private PlayerAbilities _playerAbilities;

        private bool _boostActive;
        
        public override bool PerformAction(GameObject target)
        {
            if (_boostActive) return false;
            
            Initialize(target);
            
            Debug.Log("Air ability");
            _playerAttack.DamageMultiplier = _damageMultiplier;
            _playerAttack.StartCoroutine(HandlePlayerBoost(target));

            return true;
        }

        protected override void Initialize(GameObject target)
        {
            if (_playerAttack == null)
                _playerAttack = target.GetComponent<PlayerAttack>();
            
            if (_playerAbilities == null)
                _playerAbilities = target.GetComponent<PlayerAbilities>();
        }

        private IEnumerator HandlePlayerBoost(GameObject target)
        {
            _boostActive = true;
            
            _playerAbilities.ShowBoostIcon(true);
            yield return new WaitForSeconds(AbilityDuration);
            _playerAttack.DamageMultiplier = 1f;
            _playerAbilities.ShowBoostIcon(false);

            _boostActive = false;
        }
    }
}