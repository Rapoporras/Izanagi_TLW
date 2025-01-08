using UnityEngine;

namespace PlayerController.States
{
    public class PlayerJumpingState : PlayerMovementBaseState
    {
        public PlayerJumpingState(PlayerStates key, PlayerMovement context)
            : base(key, context)
        {
            _lerpAmount = 1f;
            _canAddBonusJumpApex = true;
        }

        public override void EnterState()
        {
            Context.SetGravityScale(Context.MovementData.gravityScale);
            Context.Jump();
            Context.Audio.PlayJumpSound();
        }

        public override void UpdateState()
        {
            float gravityScale = Context.MovementData.gravityScale;
            if (Mathf.Abs(Context.Velocity.y) < Context.MovementData.jumpHangTimeThreshold)
            {
                gravityScale *= Context.MovementData.jumpHangGravityMult;
            }
            else if (!Context.HandleLongJumps)
            {
                // set higher gravity when releasing the jump button
                gravityScale *= Context.MovementData.jumpCutGravity;
            }
            
            Context.SetGravityScale(gravityScale);
        }

        public override void FixedUpdateState()
        {
            Context.Run(_lerpAmount, _canAddBonusJumpApex);
        }

        public override PlayerStates GetNextState()
        {
            if (Context.IsTakingDamage)
                return PlayerStates.Damaged;
            
            if (Context.Velocity.y < 0)
                return PlayerStates.Falling;
            
            if (Context.DashRequest && Context.CanDash && Context.AbilitiesData.airDash)
                return PlayerStates.Dashing;
            
            return StateKey;
        }
    }
}