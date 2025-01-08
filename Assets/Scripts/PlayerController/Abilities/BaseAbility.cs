using UnityEngine;

namespace PlayerController.Abilities
{
    /// <summary>
    /// <para>0 -> NoAbility</para>
    /// <para>1 -> Fire</para>
    /// <para>2 -> Ground</para>
    /// <para>3 -> Air</para>
    /// <para>4 -> Water</para>
    /// </summary>
    public enum AbilityType
    {
        NoAbility, Fire, Ground, Air, Water
    }
    
    public abstract class BaseAbility : ScriptableObject
    {
        [field: Header("General Settings")]
        [field: SerializeField] public RuntimeAnimatorController Animator { get; private set; }
        [field: SerializeField] public float CooldownDuration { get; private set; }
        
        [field: Space(10)]
        [field: SerializeField] public int AbilityMannaCost { get; private set; }
        [field: SerializeField] public float AbilityDuration { get; private set; }
        
        [field: Space(10)]
        [field: SerializeField] public int UltimateMannaCost { get; private set; }
        [field: SerializeField] public float UltimateDuration { get; private set; }
        
        public abstract AbilityType Type { get; }

        public abstract void Initialize(GameObject target);
        public abstract bool PerformAbility(GameObject target);
        public abstract bool PerformUltimate(GameObject target);
    }
}