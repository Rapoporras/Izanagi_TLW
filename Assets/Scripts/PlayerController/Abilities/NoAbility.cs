using UnityEngine;

namespace PlayerController.Abilities
{
    [CreateAssetMenu(menuName = "Player/Abilities/No Ability")]
    public class NoAbility : BaseAbility
    {
        public override AbilityType Type => AbilityType.NoAbility;
        
        public override void Initialize(GameObject target)
        {
            
        }
        
        public override bool PerformAbility(GameObject target)
        {
            Debug.Log("No Action ability");

            return true;
        }
        
        public override bool PerformUltimate(GameObject target)
        {
            return true;
        }
    }
}