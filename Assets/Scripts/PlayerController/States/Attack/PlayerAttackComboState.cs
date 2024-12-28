using UnityEngine;
using Utils.CustomLogs;

namespace PlayerController.States
{
    public class PlayerAttackComboState : PlayerAttackBaseState
    {
        private readonly float _duration;
        private float _timer;
        
        public PlayerAttackComboState(PlayerAttackStates key, PlayerAttack context)
            : base(key, context)
        {
            _duration = Context.AttackData.attackMiddleDuration;
        }

        public override void EnterState()
        {
            _timer = 0f;
            Context.SetAttackAnimation();
            Context.Audio.PlayAttackSound(1);
            
            InputManager.PlayerActions.Movement.Disable();
        }

        public override void UpdateState()
        {
            Context.ApplyDamage();
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
                
                if (Context.LastAttackInfo.Type != PlayerAttack.AttackType.Horizontal)
                    return PlayerAttackStates.AttackEntry; // upwards or downwards attack
                
                if (!Context.IsGrounded)
                    return PlayerAttackStates.AttackEntry;

                return PlayerAttackStates.AttackFinisher;
            }
            
            return StateKey;
        }
    }
}