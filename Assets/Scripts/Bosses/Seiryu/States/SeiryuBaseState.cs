using StateMachine;

namespace Bosses.States
{
    public abstract class SeiryuBaseState : BaseState<SeiryuState>
    {
        protected SeiryuController Context { get; private set; }
        
        public SeiryuBaseState(SeiryuState key, SeiryuController context) : base(key)
        {
            Context = context;
        }
    }
}