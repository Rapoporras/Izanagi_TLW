using StateMachine;

namespace Bosses.States
{
    public class InitCombatState : SeiryuBaseState
    {
        public InitCombatState(SeiryuState key, SeiryuController context)
            : base(key, context)
        {
        }

        public override SeiryuState GetNextState()
        {
            return StateKey;
        }
    }
}