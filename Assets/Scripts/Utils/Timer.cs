using System;

namespace Utils
{
    public class Timer
    {
        private float _remainingSeconds;
        private float _duration;

        public bool Finished => _remainingSeconds <= 0;
        public float RemainingSeconds => _remainingSeconds;

        public Timer(float duration)
        {
            _remainingSeconds = duration;
            _duration = duration;
        }

        public event Action OnTimerEnd;

        public void Tick(float deltaTime)
        {
            if (_remainingSeconds == 0f) return;

            _remainingSeconds -= deltaTime;
            CheckForTimerEnd();
        }

        public void Reset()
        {
            _remainingSeconds = _duration;
        }

        public void Reset(float newDuration)
        {
            _remainingSeconds = newDuration;
            _duration = newDuration;
        }

        private void CheckForTimerEnd()
        {
            if (_remainingSeconds > 0f) return;

            _remainingSeconds = 0f;
            OnTimerEnd?.Invoke();
        }
    }
}