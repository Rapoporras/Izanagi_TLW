using StateMachine;

namespace PlayerController.States
{
    public enum PlayerStates
    {
        Grounded, Jumping, Falling, WallSliding, WallJumping, Dashing, Damaged
    }
    
    public abstract class PlayerBaseState : BaseState<PlayerStates>
    {
        protected float _lerpAmount;
        protected bool _canAddBonusJumpApex;
        
        public PlayerMovement Context { get; private set; }
        
        protected PlayerBaseState(PlayerStates key, PlayerMovement context)
            : base(key)
        {
            Context = context;
        }
    }
}