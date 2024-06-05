using UnityEngine;

namespace PlayerController
{
    [CreateAssetMenu(fileName = "New PlayerAttackData", menuName = "Player Data/Attack Data")]
    public class PlayerAttackData : ScriptableObject
    {
        [Header("SETTINGS")]
        public float angleDirectionOffset;
        public int attackDamage;

        [Space(10)]
        public float horizontalKnockBackSpeed;
        public float horizontalKnockBackTime;

        [Space(10)]
        public float upwardsForce;
    }
}