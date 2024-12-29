namespace Bosses
{
    public class SeiryuTransitionState : SeiryuBaseState
    {
        public SeiryuTransitionState(SeiryuState key, SeiryuController context)
            : base(key, context) { }

        public override void EnterState()
        {
            Context.transitionToNextPhase = false;
            Context.TransitionAttack();
        }

        public override SeiryuState GetNextState()
        {
            if (Context.WaitForNextAttack)
            {
                if (Context.phase == 3)
                    return SeiryuState.Dead;
                
                return SeiryuState.Waiting;
            }
            
            return StateKey;
        }
    }
}