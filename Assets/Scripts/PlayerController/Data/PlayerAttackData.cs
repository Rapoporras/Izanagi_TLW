using UnityEngine;

namespace PlayerController.Data
{
    [CreateAssetMenu(fileName = "New PlayerAttackData", menuName = "Player/Data/Attack")]
    public class PlayerAttackData : ScriptableObject
    {
        [Header("GENERAL")]
        public float angleDirectionOffset;
        public int attackDamage;
        
        [Header("COMBO ATTACKS")]
        public float attackEntryDuration;
        public float attackMiddleDuration;
        public float attackFinisherDuration;
        
        [Header("WALL ATTACK")]
        public float wallAttackDuration;
    }
}