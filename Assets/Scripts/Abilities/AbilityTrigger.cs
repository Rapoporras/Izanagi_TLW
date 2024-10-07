using GameEvents;
using PlayerController.Abilities;
using PlayerController.Data;
using UnityEngine;

namespace Abilities
{
    public class AbilityTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerAbilitiesData _abilitiesData;
        [SerializeField] private AbilityType _abilityToUnlock;
        [SerializeField] private AbilityTypeEvent _abilityUnlockEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_abilitiesData.IsAbilityUnlock(_abilityToUnlock))
            {
                _abilitiesData.SetAbilityStatus(_abilityToUnlock, true);
                if (_abilityUnlockEvent != null)
                    _abilityUnlockEvent.Raise(_abilityToUnlock);
            }
        }
    }
}
