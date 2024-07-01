using UnityEngine;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Water")]
    public class WaterAbility : BaseAbility
    {
        public override AbilityType Type => AbilityType.Water;
        
        public override void PerformAction(GameObject target)
        {
            Debug.Log("Water ability");
        }
    }
}