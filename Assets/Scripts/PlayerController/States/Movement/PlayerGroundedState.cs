
using UnityEngine;

namespace PlayerController.States
{
    public class PlayerGroundedState : PlayerMovementBaseState
    {
        private bool _isPlayingWalkSound;
        
        public PlayerGroundedState(PlayerStates key, PlayerMovement context)
            : base(key, context)
        {
            _lerpAmount = 1f;
            _canAddBonusJumpApex = false;
        }

        public override void EnterState()
        {
            Context.ResetAdditionalJumps();
            Context.SetGravityScale(Context.MovementData.gravityScale);

            Context.IsDashActive = true;
        }

        public override void UpdateState()
        {
            if (Context.MovementDirection.x != 0)
            {
                if (!_isPlayingWalkSound)
                {
                    _isPlayingWalkSound = true;
                }
            }
            else if (_isPlayingWalkSound)
            {
                Context.Audio.StopWalkSound();
                _isPlayingWalkSound = false;
            }
        }

        public override void FixedUpdateState()
        {
            Context.Run(_lerpAmount, _canAddBonusJumpApex);
        }

        public override void ExitState()
        {
            Context.Audio.StopWalkSound();
            _isPlayingWalkSound = false;
        }

        public override PlayerStates GetNextState()
        {
            if (Context.IsTakingDamage)
                return PlayerStates.Damaged;
            
            if (!Context.IsGrounded)
            {
                Context.IsActiveCoyoteTime = true;
                return PlayerStates.Falling;
            }
            
            if (Context.JumpRequest)
            {
                Context.IsActiveCoyoteTime = false;
                return PlayerStates.Jumping;
            }

            if (Context.DashRequest && Context.CanDash)
                return PlayerStates.Dashing;
            
            return StateKey;
        }
    }
}