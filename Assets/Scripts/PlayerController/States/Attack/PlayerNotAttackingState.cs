namespace PlayerController.States
{
    public class PlayerNotAttackingState : PlayerAttackBaseState
    {
        public PlayerNotAttackingState(PlayerAttackStates key, PlayerAttack context)
            : base(key, context) { }

        public override void EnterState()
        {
            Context.ActivateAttackWindow();
        }

        public override void ExitState()
        {
            // Context.ActivateAttackWindow();
            Context.attackInput = false;
        }

        public override PlayerAttackStates GetNextState()
        {
            if (Context.attackInput)
                return PlayerAttackStates.AttackEntry;
            
            return StateKey;
        }
    }
}