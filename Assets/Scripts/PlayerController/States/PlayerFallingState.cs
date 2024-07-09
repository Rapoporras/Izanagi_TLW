using UnityEngine;

namespace PlayerController.States
{
    public class PlayerFallingState : PlayerBaseState
    {
        private float _timeInState;

        public PlayerFallingState(PlayerStates key, PlayerMovement context)
            : base(key, context)
        {
            _lerpAmount = 1f;
            _canAddBonusJumpApex = true;
        }

        public override void EnterState()
        {
            _timeInState = 0f;
        }

        public override void UpdateState()
        { 
            // coyote time
            if (_timeInState <= Context.MovementData.coyoteTime)
            {
                _timeInState += Time.deltaTime;
            }
            else
            {
                Context.IsActiveCoyoteTime = false;
            }
            
            float gravityScale = Context.MovementData.gravityScale;
            if (Context.MovementDirection.y < 0) // higher gravity if holding down
                gravityScale *= Context.MovementData.fastFallGravityMult;
            else
                gravityScale *= Context.MovementData.fallGravityMult;
                
            Context.SetGravityScale(gravityScale);
        }

        public override void FixedUpdateState()
        {
            // limit vertical velocity
            float terminalVelocity = -Context.MovementData.maxFallSpeed;
            // higher fall velocity if holding down
            if (Context.MovementDirection.y < 0)
                terminalVelocity = -Context.MovementData.maxFastFallSpeed;
            
            Context.Velocity = new Vector2(
                Context.Velocity.x,
                Mathf.Max(Context.Velocity.y, terminalVelocity));
            
            Context.Run(_lerpAmount, _canAddBonusJumpApex);
        }

        public override void ExitState()
        {
            Context.IsActiveCoyoteTime = false;
        }

        public override PlayerStates GetNextState()
        {
            if (Context.IsTakingDamage)
                return PlayerStates.Damaged;
            
            if (Context.IsGrounded)
                return PlayerStates.Grounded;
            
            if (Context.JumpRequest)
            {
                if (Context.IsActiveCoyoteTime)
                    return PlayerStates.Jumping;

                if (Context.CanPerformExtraJump())
                {
                    Context.AdditionalJumpsAvailable--;
                    return PlayerStates.Jumping;
                }
            }

            if (Context.CanWallSlide())
                return PlayerStates.WallSliding;
            
            if (Context.DashRequest && Context.CanDash && Context.AbilitiesData.airDash)
                return PlayerStates.Dashing;
            
            return StateKey;
        }
    }
}