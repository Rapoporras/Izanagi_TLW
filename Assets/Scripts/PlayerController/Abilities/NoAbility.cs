using UnityEngine;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/No Ability")]
    public class NoAbility : BaseAbility
    {
        public override AbilityType Type => AbilityType.NoAbility;
        
        public override void PerformAction(GameObject target)
        {
            Debug.Log("No Action ability");
        }
    }
}