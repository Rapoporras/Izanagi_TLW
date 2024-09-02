using UnityEngine;

namespace SceneMechanics.Stalactite
{
    public class StalactiteDetachingState : StalactiteBaseState
    {
        private float _timer;
        
        public StalactiteDetachingState(StalactiteStates key, Stalactite context)
            : base(key, context) { }

        public override void EnterState()
        {
            _timer = 0f;
            Context.SetColor();
            
            // add some effects or animations
        }

        public override void UpdateState()
        {
            _timer += Time.deltaTime;
        }

        public override void FixedUpdateState() { }

        public override void ExitState() { }

        public override StalactiteStates GetNextState()
        {
            if (_timer >= Context.detachingDuration)
                return StalactiteStates.Falling;
            
            return StateKey;
        }
    }
}