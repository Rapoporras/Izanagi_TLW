using UnityEngine;

namespace PlayerController
{
    [CreateAssetMenu(fileName = "New PlayerAttackData", menuName = "Player/Data/Attack")]
    public class PlayerAttackData : ScriptableObject
    {
        [Header("SETTINGS")]
        public float angleDirectionOffset;
        public int attackDamage;
    }
}