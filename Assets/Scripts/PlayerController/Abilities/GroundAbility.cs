using UnityEngine;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Ground")]
    public class GroundAbility : BaseAbility
    {
        public override AbilityType Type => AbilityType.Ground;
        
        public override void PerformAction(GameObject target)
        {
            Debug.Log("Ground ability");
        }
    }
}