using PlayerController.Abilities;
using UnityEngine;

namespace PlayerController.Data
{
    [CreateAssetMenu(fileName = "New PlayerAbilitiesData", menuName = "Player/Data/Abilities")]
    public class PlayerAbilitiesData : ScriptableObject
    {
        [Header("FIRE")]
        public bool doubleJump;
        public bool fireAbility;
        
        [Header("GROUND")]
        public bool groundAbility;
        
        [Header("AIR")]
        public bool airDash;
        public bool airAbility;
        
        [Header("WATER")]
        public bool wallImpulse;
        public bool waterAbility;

        public void UnlockAbility(AbilityType type)
        {
            switch (type)
            {
                case AbilityType.Air:
                    airDash = true;
                    airAbility = true;
                    break;
                case AbilityType.Fire:
                    doubleJump = true;
                    fireAbility = true;
                    break;
                case AbilityType.Ground:
                    groundAbility = true;
                    break;
                case AbilityType.Water:
                    wallImpulse = true;
                    waterAbility = true;
                    break;
            }
        }

        public bool IsAbilityUnlock(AbilityType type)
        {
            switch (type)
            {
                case AbilityType.Air:
                    return airDash && airAbility;
                case AbilityType.Fire:
                    return doubleJump && fireAbility;
                case AbilityType.Ground:
                    return groundAbility;
                case AbilityType.Water:
                    return wallImpulse && waterAbility;
                default:
                    return false;
            }
        }
    }
}
