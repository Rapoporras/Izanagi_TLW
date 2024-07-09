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
            Context.dashInvulnerability.Value = true;

            if (Context.MovementDirection.x != 0f)
                _direction = Context.MovementDirection.x < 0 ? Vector2.left : Vector2.right;
            else
                _direction = Context.IsFacingRight ? Vector2.right : Vector2.left;
            
            Context.SetDirectionToFace(_direction.x > 0);
            
            InputManager.Instance.PlayerActions.Attack.Disable();
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
            Context.dashInvulnerability.Value = false;
            
            InputManager.Instance.PlayerActions.Attack.Enable();
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