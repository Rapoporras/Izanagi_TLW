using UnityEditor.Timeline;

namespace Bosses
{
    public class SeiryuCombatState : SeiryuBaseState
    {
        public SeiryuCombatState(SeiryuState key, SeiryuController context)
            : base(key, context) { }

        public override void EnterState()
        {
            if (Context.transitionToNextPhase)
                Context.TransitionAttack();
            else
                Context.TryToAttack();
        }

        public override SeiryuState GetNextState()
        {
            if (Context.WaitForNextAttack)
            {
                if (Context.phase == 3 && !Context.transitionToNextPhase)
                    return SeiryuState.Dead;
                
                return SeiryuState.Waiting;
            }
            
            return StateKey;
        }
    }
}