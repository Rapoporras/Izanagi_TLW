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
        public bool breakWalls;
        public bool groundAbility;
        
        [Header("AIR")]
        public bool airDash;
        public bool airAbility;
        
        [Header("WATER")]
        public bool wallImpulse;
        public bool waterAbility;

        [ContextMenu("Activate FIRE")]
        public void ActivateFireAbilities()
        {
            doubleJump = true;
            fireAbility = true;
        }

        [ContextMenu("Deactivate FIRE")]
        public void DeactivateFireAbilities()
        {
            doubleJump = false;
            fireAbility = false;
        }
        
        [ContextMenu("Activate GROUND")]
        public void ActivateGroundAbilities()
        {
            breakWalls = true;
            groundAbility = true;
        }

        [ContextMenu("Deactivate GROUND")]
        public void DeactivateGroundAbilities()
        {
            breakWalls = false;
            groundAbility = false;
        }
        
        [ContextMenu("Activate AIR")]
        public void ActivateAirAbilities()
        {
            airDash = true;
            airAbility = true;
        }

        [ContextMenu("Deactivate AIR")]
        public void DeactivateAirAbilities()
        {
            airDash = false;
            airAbility = false;
        }
        
        [ContextMenu("Activate WATER")]
        public void ActivateWaterAbilities()
        {
            wallImpulse = true;
            waterAbility = true;
        }

        [ContextMenu("Deactivate WATER")]
        public void DeactivateWaterAbilities()
        {
            wallImpulse = false;
            waterAbility = false;
        }
    }
}
