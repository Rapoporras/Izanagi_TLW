namespace PlayerController.States
{
    public class PlayerNotAttackingState : PlayerAttackBaseState
    {
        public PlayerNotAttackingState(PlayerAttackStates key, PlayerAttack context)
            : base(key, context) { }

        public override void EnterState() { }

        public override void UpdateState() { }

        public override void FixedUpdateState() { }

        public override void ExitState()
        {
            Context.ActivateAttackWindow();
            Context.attackInput = false;
            Context.wallAttackInput = false;
        }

        public override PlayerAttackStates GetNextState()
        {
            if (Context.attackInput)
                return PlayerAttackStates.AttackEntry;
            
            if (Context.wallAttackInput)
                return PlayerAttackStates.WallAttack;
            
            return StateKey;
        }
    }
}