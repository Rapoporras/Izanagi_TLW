using UnityEngine;
using Utils;

namespace Bosses
{
    public class SeiryuDeadState : SeiryuBaseState
    {
        private readonly float _deathDuration;
        private readonly Timer _timer;

        private bool _hasFinishedBattle;

        public SeiryuDeadState(SeiryuState key, SeiryuController context)
            : base(key, context)
        {
            _deathDuration = 1f;
            _timer = new Timer(_deathDuration);

            _hasFinishedBattle = false;
        }

        public override void EnterState()
        {
            // TODO - añadir algun sonido o efecto visual
            
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
            else if (!_hasFinishedBattle)
            {
                Context.FinishBattle();
                _hasFinishedBattle = true;
            }
        }

        public override SeiryuState GetNextState()
        {
            return StateKey;
        }
    }
}