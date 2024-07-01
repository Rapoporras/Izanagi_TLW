using UnityEngine;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/Water")]
    public class WaterAbility : BaseAbility
    {
        public override AbilityType Type => AbilityType.Water;
        
        public override bool PerformAction(GameObject target)
        {
            Debug.Log("Water ability");

            return true;
        }

        protected override void Initialize(GameObject target)
        {
            
        }
    }
}