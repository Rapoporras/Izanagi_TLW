using UnityEngine;

namespace SceneMechanics.Stalactite
{
    public class StalactiteFallingState : StalactiteBaseState
    {
        public StalactiteFallingState(StalactiteStates key, Stalactite context)
            : base(key, context) { }

        public override void EnterState()
        {
            Context.DamageActive = true;
            Context.SetGravityScale(Context.fallGravityScale);
            
            Context.SetColor();
        }

        public override void UpdateState() { }

        public override void FixedUpdateState()
        {
            Context.Velocity = new Vector2(
                Context.Velocity.x,
                Mathf.Max(Context.Velocity.y, Context.maxFallingVelocity * -1f));
        }

        public override void ExitState()
        {
            Context.DamageActive = false;
        }

        public override StalactiteStates GetNextState()
        {
            if (Context.IsGrounded)
                return StalactiteStates.Grounded;
            
            return StateKey;
        }
    }
}