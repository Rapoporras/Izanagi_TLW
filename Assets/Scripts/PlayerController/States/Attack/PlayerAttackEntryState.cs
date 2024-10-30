using Unity.VisualScripting;
using UnityEngine;
using Utils.CustomLogs;

namespace PlayerController.States
{
    // upwards and downwards attacks only have entry state
    public class PlayerAttackEntryState : PlayerAttackBaseState
    {
        private readonly float _duration;
        private float _timer;
        
        public PlayerAttackEntryState(PlayerAttackStates key, PlayerAttack context)
            : base(key, context)
        {
            _duration = Context.AttackData.attackEntryDuration;
        }

        public override void EnterState()
        {
            _timer = 0f;
            Context.SetAttackAnimation();
            Context.Audio.PlayAttackSound(0);
            
            InputManager.Instance.PlayerActions.Movement.Disable();
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

                // upwards or downwards attack
                if (Context.LastAttackInfo.Type != PlayerAttack.AttackType.Horizontal)
                    return ResetState();

                if (!Context.IsGrounded)
                    return ResetState();

                return PlayerAttackStates.AttackCombo;
            }
            
            return StateKey;
        }
    }
}