using UnityEngine;

namespace PlayerController.States
{
    public class PlayerWallImpulseState : PlayerBaseState
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
            
            Debug.Log("CHARGING WALL IMPULSE");
            
            // freeze player movement
            Context.SetGravityScale(0f);
            Context.Velocity = Vector2.zero;
            
            // set direction
            Context.SetDirectionToFace(Context.LeftWallHit);
            _direction = Context.LeftWallHit ? Vector2.right : Vector2.left;
        }

        public override void UpdateState()
        {
            if (_timeInState < Context.MovementData.wallImpulseChargeTime)
                _timeInState += Time.deltaTime;
            else
            {
                Debug.Log("IMPULSE CHARGED");
                if (!Context.HandleWallImpulse && !_impulseReleased)
                {
                    _impulseReleased = true;
                    Debug.Log("IMPULSE");
                    Context.Velocity = _direction * Context.MovementData.wallImpulseVelocity;
                }
            }
        }

        public override void FixedUpdateState() { }

        public override void ExitState() { }

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