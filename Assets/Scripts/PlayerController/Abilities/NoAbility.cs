using UnityEngine;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/No Ability")]
    public class NoAbility : BaseAbility
    {
        public override AbilityType Type => AbilityType.NoAbility;
        
        public override bool PerformAction(GameObject target)
        {
            Debug.Log("No Action ability");

            return true;
        }

        protected override void Initialize(GameObject target)
        {
            
        }
    }
}