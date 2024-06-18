using UnityEngine;

namespace PlayerController
{
    [CreateAssetMenu(fileName = "New PlayerAbilitiesData", menuName = "Player/Data/Abilities")]
    public class PlayerAbilitiesData : ScriptableObject
    {
        [Header("FIRE")]
        public bool doubleJump;
        public bool basicFireAttack;
        public bool ultimateFireAttack;
        
        [Header("GROUND")]
        public bool breakWalls;
        public bool basicGroundAttack;
        public bool ultimateGroundAttack;
        
        [Header("AIR")]
        public bool airDash;
        public bool basicAirAttack;
        public bool ultimateAirAttack;
        
        [Header("WATER")]
        public bool wallJump;
        public bool basicWaterAttack;
        public bool ultimateWaterAttack;

        [ContextMenu("Activate FIRE")]
        public void ActivateFireAbilities()
        {
            doubleJump = true;
            basicFireAttack = true;
            ultimateFireAttack = true;
        }

        [ContextMenu("Deactivate FIRE")]
        public void DeactivateFireAbilities()
        {
            doubleJump = false;
            basicFireAttack = false;
            ultimateFireAttack = false;
        }
        
        [ContextMenu("Activate GROUND")]
        public void ActivateGroundAbilities()
        {
            breakWalls = true;
            basicGroundAttack = true;
            ultimateGroundAttack = true;
        }

        [ContextMenu("Deactivate GROUND")]
        public void DeactivateGroundAbilities()
        {
            breakWalls = false;
            basicGroundAttack = false;
            ultimateGroundAttack = false;
        }
        
        [ContextMenu("Activate AIR")]
        public void ActivateAirAbilities()
        {
            airDash = true;
            basicAirAttack = true;
            ultimateAirAttack = true;
        }

        [ContextMenu("Deactivate AIR")]
        public void DeactivateAirAbilities()
        {
            airDash = false;
            basicAirAttack = false;
            ultimateAirAttack = false;
        }
        
        [ContextMenu("Activate WATER")]
        public void ActivateWaterAbilities()
        {
            wallJump = true;
            basicWaterAttack = true;
            ultimateWaterAttack = true;
        }

        [ContextMenu("Deactivate WATER")]
        public void DeactivateWaterAbilities()
        {
            wallJump = false;
            basicWaterAttack = false;
            ultimateWaterAttack = false;
        }
    }
}
