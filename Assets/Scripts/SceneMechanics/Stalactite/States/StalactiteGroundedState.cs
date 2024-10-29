
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

        public override void UpdateState() { }

        public override void FixedUpdateState() { }

        public override void ExitState() { }

        public override StalactiteStates GetNextState()
        {
            return StateKey;
        }
    }
}