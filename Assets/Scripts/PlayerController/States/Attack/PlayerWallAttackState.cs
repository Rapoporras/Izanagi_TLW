using UnityEngine;

namespace PlayerController.States
{
    public class PlayerWallAttackState : PlayerAttackBaseState
    {
        private float _duration;
        private float _timer;
        
        public PlayerWallAttackState(PlayerAttackStates key, PlayerAttack context)
            : base(key, context)
        {
            _duration = Context.AttackData.wallAttackDuration;
        }

        public override void EnterState()
        {
            _timer = 0f;
            Context.SetWallAttackAnimation();
        }

        public override void UpdateState()
        {
            Context.CheckBreakableWalls();
            _timer += Time.deltaTime;
        }

        public override void FixedUpdateState() { }

        public override void ExitState()
        {
            Context.StopAttack();
        }

        public override PlayerAttackStates GetNextState()
        {
            if (_timer >= _duration)
            {
                if (!Context.attackInput)
                    return PlayerAttackStates.NotAttacking;

                return ResetState();
            }
            
            return StateKey;
        }
    }
}