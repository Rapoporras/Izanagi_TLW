using GlobalVariables;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerController.Data
{
    [CreateAssetMenu(fileName = "New PlayerAttackData", menuName = "Player/Data/Attack")]
    public class PlayerAttackData : ScriptableObject
    {
        [Header("GENERAL")]
        public float angleDirectionOffset = 30f;
        public int baseAttackDamage = 10;
        public FloatReference attackMultiplier;
        public int AttackDamage => Mathf.RoundToInt(baseAttackDamage * attackMultiplier);
        
        [Header("COMBO ATTACKS")]
        public float attackEntryDuration = 0.4f;
        public float attackMiddleDuration = 0.4f;
        public float attackFinisherDuration = 0.4f;
    }
}