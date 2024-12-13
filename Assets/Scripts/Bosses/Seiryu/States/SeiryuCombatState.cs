using Utils;

namespace Bosses.States
{
    public class SeiryuCombatState : SeiryuBaseState
    {
        private readonly float _attackDelayFirstPhase;
        
        private Timer _attacksTimer;
        
        public SeiryuCombatState(SeiryuState key, SeiryuController context) : base(key, context)
        {
            _attackDelayFirstPhase = 5f;
            _attacksTimer = new Timer(_attackDelayFirstPhase);
        }

        public override void EnterState()
        {
            
        }

        public override SeiryuState GetNextState()
        {
            return StateKey;
        }
    }
}