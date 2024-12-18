namespace Bosses
{
    public class SeiryuDeadState : SeiryuBaseState
    {
        public SeiryuDeadState(SeiryuState key, SeiryuController context)
            : base(key, context) { }

        public override SeiryuState GetNextState()
        {
            return StateKey;
        }
    }
}