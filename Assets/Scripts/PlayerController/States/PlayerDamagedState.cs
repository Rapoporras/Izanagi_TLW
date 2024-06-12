using UnityEngine;

namespace PlayerController.States
{
    public class PlayerDamagedState : PlayerBaseState
    {
        private float _timeInState;
        
        public PlayerDamagedState(PlayerStates key, PlayerMovement context)
            : base(key, context) { }

        public override void EnterState()
        {
            _timeInState = 0f;
        }

        public override void UpdateState()
        {
            _timeInState += Time.deltaTime;
        }

        public override void FixedUpdateState() { }

        public override void ExitState()
        {
            Context.IsTakingDamage = false;
            Context.UseKnockBackAccelInAir = true;
        }

        public override PlayerStates GetNextState()
        {
            if (_timeInState >= Context.Data.knockBackDuration)
            {
                if (Context.IsGrounded)
                    return PlayerStates.Grounded;
                
                return PlayerStates.Falling;
            }
            
            return StateKey;
        }
    }
}