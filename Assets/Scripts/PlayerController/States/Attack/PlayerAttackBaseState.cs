using StateMachine;

namespace PlayerController.States
{
    public enum PlayerAttackStates
    {
        NotAttacking, AttackEntry, AttackCombo, AttackFinisher, WallAttack
    }
    
    public abstract class PlayerAttackBaseState : BaseState<PlayerAttackStates>
    {
        protected PlayerAttack Context { get; private set; }
        
        protected PlayerAttackBaseState(PlayerAttackStates key, PlayerAttack context)
            : base(key)
        {
            Context = context;
        }
    }
}