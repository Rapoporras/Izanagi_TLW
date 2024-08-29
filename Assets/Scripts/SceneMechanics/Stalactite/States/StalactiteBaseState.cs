using StateMachine;

namespace SceneMechanics.Stalactite
{
    public enum StalactiteStates
    {
        Idle, Detaching, Falling, Grounded
    }
    
    public abstract class StalactiteBaseState : BaseState<StalactiteStates>
    {
        protected Stalactite Context { get; private set; }
        
        protected StalactiteBaseState(StalactiteStates key, Stalactite context)
            : base(key)
        {
            Context = context;
        }
    }
}