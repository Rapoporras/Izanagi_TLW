using UnityEngine;

namespace PlayerController.States
{
    public class PlayerDashingState : PlayerBaseState
    {
        private float _timeInState;
        private Vector2 _direction;

        public PlayerDashingState(PlayerStates key, PlayerMovement context)
            : base(key, context) { }

        public override void EnterState()
        {
            _timeInState = 0f;
            
            Context.IsDashActive = false;

            if (Context.MovementDirection.x != 0f)
                _direction = Context.MovementDirection.x < 0 ? Vector2.left : Vector2.right;
            else
                _direction = Context.IsFacingRight ? Vector2.right : Vector2.left;
            
            Context.SetDirectionToFace(_direction.x > 0);
        }

        public override void UpdateState()
        {
            _timeInState += Time.deltaTime;
            Context.Velocity = _direction * Context.MovementData.dashSpeed;
        }

        public override void FixedUpdateState() { }

        public override void ExitState()
        {
            Context.RefillDash();
        }

        public override PlayerStates GetNextState()
        {
            if (Context.IsTakingDamage)
                return PlayerStates.Damaged;
            
            if (_timeInState >= Context.MovementData.dashTime)
            {
                if (Context.IsGrounded)
                    return PlayerStates.Grounded;
                
                return PlayerStates.Falling;
            }
            
            return StateKey;
        }
    }
}