using UnityEngine;
using Utils;

namespace Bosses
{
    public class SeiryuInitCombatState : SeiryuBaseState
    {
        private readonly float _waitingTime;
        private Timer _timer;
        
        public SeiryuInitCombatState(SeiryuState key, SeiryuController context)
            : base(key, context)
        {
            _waitingTime = 3f;
            _timer = new Timer(_waitingTime);
        }

        public override void EnterState()
        {
            // TODO - añadir algún sonido
            
            Context.DeactivateHealth();
            _timer.Reset();
        }

        public override void UpdateState()
        {
            if (Context.CanStartFight)
            {
                _timer.Tick(Time.deltaTime);
                
                float alpha = 1 - (_timer.RemainingSeconds / _waitingTime);
                Context.SetSpritesAlpha(alpha);
            }
        }

        public override void ExitState()
        {
            Context.ActivateHealth();
        }

        public override SeiryuState GetNextState()
        {
            if (_timer.Finished)
                return SeiryuState.Combat;
            
            return StateKey;
        }
    }
}