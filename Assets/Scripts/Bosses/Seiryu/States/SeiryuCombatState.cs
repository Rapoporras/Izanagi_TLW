namespace Bosses.States
{
    public class SeiryuCombatState : SeiryuBaseState
    {
        public SeiryuCombatState(SeiryuState key, SeiryuController context) : base(key, context)
        {
        }

        public override SeiryuState GetNextState()
        {
            return StateKey;
        }
    }
}