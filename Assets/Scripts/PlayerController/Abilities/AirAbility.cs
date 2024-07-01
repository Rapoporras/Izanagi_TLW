using UnityEngine;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Air")]
    public class AirAbility : BaseAbility
    {
        public override AbilityType Type => AbilityType.Air;
        
        public override void PerformAction(GameObject target)
        {
            Debug.Log("Air ability");
        }
    }
}