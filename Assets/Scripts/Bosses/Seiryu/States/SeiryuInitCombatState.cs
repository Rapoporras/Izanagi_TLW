using UnityEngine;
using Utils;

namespace Bosses.States
{
    // TODO - añadir animacion de inicio de batalla
    
    public class SeiryuInitCombatState : SeiryuBaseState
    {
        private readonly float _waitTimer;
        private Timer _timer;
        
        public SeiryuInitCombatState(SeiryuState key, SeiryuController context)
            : base(key, context)
        {
            _waitTimer = 30f;
            _timer = new Timer(_waitTimer);
        }

        public override void EnterState()
        {
            _timer.Reset();
        }

        public override void UpdateState()
        {
            if (Context.CanStartFight)
            {
                _timer.Tick(Time.deltaTime);
            }
        }

        public override SeiryuState GetNextState()
        {
            if (_timer.Finished)
                return SeiryuState.Combat;
            
            return StateKey;
        }
    }
}