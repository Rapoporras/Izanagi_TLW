using UnityEngine;
using Utils.CustomLogs;

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
            LogManager.Log("No Action ability", FeatureType.Abilities);
            return true;
        }
        
        public override bool PerformUltimate(GameObject target)
        {
            return true;
        }
    }
}