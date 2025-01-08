
namespace SceneMechanics.Stalactite
{
    public class StalactiteGroundedState : StalactiteBaseState
    {
        public StalactiteGroundedState(StalactiteStates key, Stalactite context)
            : base(key, context) { }

        public override void EnterState()
        {
            Context.SetColor();
            Context.PlayFloorHitAudio();
        }

        public override StalactiteStates GetNextState()
        {
            return StateKey;
        }
    }
}