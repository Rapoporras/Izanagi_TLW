using System;
using System.Collections.Generic;
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

        public void SetAbilityStatus(AbilityType type, bool unlocked)
        {
            switch (type)
            {
                case AbilityType.Air:
                    airDash = unlocked;
                    airAbility = unlocked;
                    break;
                case AbilityType.Fire:
                    doubleJump = unlocked;
                    fireAbility = unlocked;
                    break;
                case AbilityType.Ground:
                    groundAbility = unlocked;
                    break;
                case AbilityType.Water:
                    wallImpulse = unlocked;
                    waterAbility = unlocked;
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

        public List<int> GetAbilitiesList()
        {
            List<int> unlockedAbilities = new List<int>();
            AbilityType[] abilities = (AbilityType[])Enum.GetValues(typeof(AbilityType));

            foreach (var ability in abilities)
            {
                if (IsAbilityUnlock(ability))
                    unlockedAbilities.Add((int) ability);
            }
            
            return unlockedAbilities;
        }

        public AbilityType GetFirstAbilityUnlocked()
        {
            if (IsAbilityUnlock(AbilityType.Air))
                return AbilityType.Air;
            if (IsAbilityUnlock(AbilityType.Fire))
                return AbilityType.Fire;
            if (IsAbilityUnlock(AbilityType.Ground))
                return AbilityType.Ground;
            if (IsAbilityUnlock(AbilityType.Water))
                return AbilityType.Water;
            
            return AbilityType.NoAbility;
        }
    }
}
