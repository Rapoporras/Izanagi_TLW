using Utils;

namespace Bosses
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
            Context.TryToAttack();
        }

        public override SeiryuState GetNextState()
        {
            if (Context.WaitForNextAttack)
            {
                if (Context.transitionToNextPhase)
                    return SeiryuState.Transition;
                
                return SeiryuState.Waiting;
            }
            
            return StateKey;
        }
    }
}