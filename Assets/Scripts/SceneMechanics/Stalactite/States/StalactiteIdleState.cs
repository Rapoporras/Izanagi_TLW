
namespace SceneMechanics.Stalactite
{
    public class StalactiteIdleState : StalactiteBaseState
    {
        public StalactiteIdleState(StalactiteStates key, Stalactite context)
            : base(key, context) { }

        public override void EnterState()
        {
            Context.playerDetected = false;
            Context.DamageActive = false;
            Context.SetGravityScale(0f);
            
            Context.SetColor();
        }

        public override void UpdateState()
        {
            Context.CheckRaycasts();
        }

        public override StalactiteStates GetNextState()
        {
            if (Context.playerDetected)
                return StalactiteStates.Detaching;
            
            return StateKey;
        }
    }
}