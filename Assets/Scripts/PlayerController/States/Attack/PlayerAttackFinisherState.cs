using UnityEngine;
using Utils.CustomLogs;

namespace PlayerController.States
{
    public class PlayerAttackFinisherState : PlayerAttackBaseState
    {
        private readonly float _duration;
        private float _timer;
        
        public PlayerAttackFinisherState(PlayerAttackStates key, PlayerAttack context)
            : base(key, context)
        {
            _duration = Context.AttackData.attackFinisherDuration;
        }

        public override void EnterState()
        {
            _timer = 0f;
            Context.SetAttackAnimation();
            Context.Audio.PlayAttackSound(2);
            
            Context.DisableMovement();
        }

        public override void UpdateState()
        {
            Context.ApplyDamage();
            _timer += Time.deltaTime;
        }

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

                return PlayerAttackStates.AttackEntry;
            }
            
            return StateKey;
        }
    }
}