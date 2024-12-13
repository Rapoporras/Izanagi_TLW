using UnityEngine;
using Utils;

namespace Bosses.States
{
    public class SeiryuWaitingState : SeiryuBaseState
    {
        private Timer _timer;

        public SeiryuWaitingState(SeiryuState key, SeiryuController context)
            : base(key, context)
        {
            _timer = new Timer(1f);
        }

        public override void EnterState()
        {
            _timer.Reset(Context.GetAttackWaitingTime());
        }

        public override void UpdateState()
        {
            _timer.Tick(Time.deltaTime);
        }

        public override SeiryuState GetNextState()
        {
            if (_timer.Finished)
                return SeiryuState.Combat;
                
            return StateKey;
        }
    }
}