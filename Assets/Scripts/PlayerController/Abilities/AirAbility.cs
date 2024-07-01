using System.Collections;
using UnityEngine;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Air")]
    public class AirAbility : BaseAbility
    {
        [SerializeField, Range(1f, 2f)] private float _damageMultiplier;
        
        public override AbilityType Type => AbilityType.Air;
        
        public override void PerformAction(GameObject target)
        {
            if (target.TryGetComponent(out PlayerAttack playerAttack))
            {
                Debug.Log("Air ability");
                playerAttack.DamageMultiplier = _damageMultiplier;
                playerAttack.StartCoroutine(HandlePlayerBoost(playerAttack));
            }
        }

        private IEnumerator HandlePlayerBoost(PlayerAttack playerAttack)
        {
            yield return new WaitForSeconds(AbilityDuration);
            playerAttack.DamageMultiplier = 1f;
        }
    }
}