using StateMachine;

namespace PlayerController.States
{
    public enum PlayerStates
    {
        Grounded, Jumping, Falling, WallSliding, WallJumping, Dashing, Damaged, WallImpulse
    }
    
    public abstract class PlayerMovementBaseState : BaseState<PlayerStates>
    {
        protected float _lerpAmount;
        protected bool _canAddBonusJumpApex;
        
        protected PlayerMovement Context { get; private set; }
        
        protected PlayerMovementBaseState(PlayerStates key, PlayerMovement context)
            : base(key)
        {
            Context = context;
        }
    }
}