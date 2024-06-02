﻿using UnityEngine;

namespace PlayerController.States
{
    public class PlayerWallSlidingState : PlayerBaseState
    {
        private bool _leftSide;
        private float _movingTimer;
        
        public PlayerWallSlidingState(PlayerStates key, PlayerMovement context)
            : base(key, context)
        {
            _lerpAmount = 1f;
            _canAddBonusJumpApex = false;
        }

        public override void EnterState()
        {
            _movingTimer = Context.Data.wallSlideReleaseTime;
            _leftSide = Context.LeftWallHit;
            
            Context.ResetAdditionalJumps();
            Context.SetGravityScale(0);
        }

        public override void UpdateState()
        {
            if (Context.MovementDirection.x > 0 && _leftSide
                || Context.MovementDirection.x < 0 && !_leftSide)
            {
                _movingTimer -= Time.deltaTime;
            }
            else
            {
                _movingTimer = Context.Data.wallSlideReleaseTime;
            }
        }

        public override void FixedUpdateState()
        {
            Context.Slide();
            
            if (_movingTimer <= 0)
                Context.Run(_lerpAmount, _canAddBonusJumpApex);
        }

        public override void ExitState() { }

        public override PlayerStates GetNextState()
        {
            if (Context.IsGrounded)
                return PlayerStates.Grounded;

            if (Context.JumpRequest)
                return PlayerStates.WallJumping;

            if (!Context.LeftWallHit && !Context.RightWallHit)
                return PlayerStates.Falling;
            
            return PlayerStates.WallSliding;
        }
    }
}