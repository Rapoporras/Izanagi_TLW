using UnityEngine;
using Utils;

namespace Bosses
{
    public class SeiryuDeadState : SeiryuBaseState
    {
        private readonly float _deathDuration;
        private Timer _timer;

        public SeiryuDeadState(SeiryuState key, SeiryuController context)
            : base(key, context)
        {
            _deathDuration = 1f;
            _timer = new Timer(_deathDuration);
        }

        public override void EnterState()
        {
            _timer.Reset();
        }

        public override void UpdateState()
        {
            if (!_timer.Finished)
            {
                _timer.Tick(Time.deltaTime);
                
                float alpha = _timer.RemainingSeconds / _deathDuration;
                Context.SetSpritesAlpha(alpha);
            }
            else
            {
                Context.FinishBattle();
            }
        }

        public override SeiryuState GetNextState()
        {
            return StateKey;
        }
    }
}