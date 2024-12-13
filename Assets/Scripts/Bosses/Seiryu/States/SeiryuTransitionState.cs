namespace Bosses.States
{
    public class SeiryuTransitionState : SeiryuBaseState
    {
        public SeiryuTransitionState(SeiryuState key, SeiryuController context)
            : base(key, context) { }

        public override SeiryuState GetNextState()
        {
            return StateKey;
        }
    }
}