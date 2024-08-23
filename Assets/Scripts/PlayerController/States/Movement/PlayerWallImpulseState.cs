using UnityEngine;

namespace PlayerController.States
{
    public class PlayerWallImpulseState : PlayerMovementBaseState
    {
        private float _timeInState;
        private bool _impulseReleased;
        private Vector2 _direction;
        
        public PlayerWallImpulseState(PlayerStates key, PlayerMovement context) 
            : base(key, context) { }

        public override void EnterState()
        {
            _timeInState = 0f;
            _impulseReleased = false;
            
            // freeze player movement
            Context.SetGravityScale(0f);
            Context.Velocity = Vector2.zero;
            
            // set direction
            Context.SetDirectionToFace(Context.LeftWallHit);
            _direction = Context.LeftWallHit ? Vector2.right : Vector2.left;
            
            InputManager.Instance.PlayerActions.Attack.Disable();
            
            Context.wallImpulseArrow.SetActive(false);
        }

        public override void UpdateState()
        {
            if (_timeInState < Context.MovementData.wallImpulseChargeTime)
                _timeInState += Time.deltaTime;
            else
            {
                Context.wallImpulseArrow.SetActive(true);
                if (!Context.HandleWallImpulse && !_impulseReleased)
                {
                    _impulseReleased = true;
                    Context.Velocity = _direction * Context.MovementData.wallImpulseVelocity;
                }
            }
        }

        public override void FixedUpdateState() { }

        public override void ExitState()
        {
            InputManager.Instance.PlayerActions.Attack.Enable();
            Context.wallImpulseArrow.SetActive(false);
        }

        public override PlayerStates GetNextState()
        {
            if (Context.IsTakingDamage)
                return PlayerStates.Damaged;
            
            if (!Context.HandleWallImpulse && _timeInState < Context.MovementData.wallImpulseChargeTime)
            {
                Context.SetDirectionToFace(Context.RightWallHit);
                return PlayerStates.WallSliding;
            }

            if (CheckCollision() && _impulseReleased)
                return PlayerStates.WallSliding;

            if (Context.HandleWallImpulse && _impulseReleased)
                return PlayerStates.Falling;
            
            return StateKey;
        }

        private bool CheckCollision()
        {
            if (_direction.x > 0)
                return Context.RightWallHit;
            else
                return Context.LeftWallHit;
        }
    }
}