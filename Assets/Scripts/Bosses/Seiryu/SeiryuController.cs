using StateMachine;

namespace Bosses
{
    public enum SeiryuState
    {
        Init, Combat, Transition, Dead
    }
    
    public class SeiryuController : BaseStateMachine<SeiryuState>
    {
        protected override void SetStates()
        {
            
        }
    }
}