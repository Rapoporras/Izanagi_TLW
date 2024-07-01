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
        
        public override void PerformAction(GameObject target)
        {
            if (!target.GetComponent<PlayerMovement>().IsGrounded) return;
            
            Debug.Log("Fire ability");
            FireAttack attack = Instantiate(_fireAttackPrefab, target.transform.position, target.transform.rotation);
            attack.Damage = _damage;
            attack.Duration = AbilityDuration;
            
            target.GetComponent<PlayerAbilities>().StartCoroutine(HandlePlayerInput());
        }

        private IEnumerator HandlePlayerInput()
        {
            // TODO: freeze player in current position
            
            InputManager.Instance.PlayerActions.Disable();
            yield return new WaitForSeconds(AbilityDuration);
            InputManager.Instance.PlayerActions.Enable();
        }
    }
}