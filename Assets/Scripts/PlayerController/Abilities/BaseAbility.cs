using UnityEngine;

namespace PlayerController.Abilities
{
    public enum AbilityType
    {
        NoAbility, Fire, Ground, Air, Water
    }
    
    public abstract class BaseAbility : ScriptableObject
    {
        [field: SerializeField] public RuntimeAnimatorController Animator { get; private set; }
        [field: SerializeField] public float CooldownDuration { get; private set; }
        [field: SerializeField] public int MannaCost { get; private set; }
        [field: SerializeField] public float AbilityDuration { get; private set; }
        
        public abstract AbilityType Type { get; }

        public abstract void PerformAction(GameObject target);
    }
}