using UnityEngine;

namespace Bosses
{
    public class SeiryuHead : MonoBehaviour, ISeiryuAttackStateHandler
    {
        private Transform _player;
        private Animator _animator;

        private int _defaultAnimHash;
        private int _angerAnimHash;

        private float maxAngle = 20f;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _defaultAnimHash = Animator.StringToHash("default");
            _angerAnimHash = Animator.StringToHash("anger");
        }

        public void Initialize(Transform player)
        {
            _player = player;
        }

        private void Update()
        {
            LookAtPlayer();
        }

        private void LookAtPlayer()
        {
            if (!_player) return;

            Vector3 dir = _player.position - transform.position;
            dir *= -1;
            transform.up = dir;
        }

        public void OnAttackStateChange(SeiryuAttackInfo info)
        {
            switch (info.state)
            {
                case AttackState.StartAttack:
                    _animator.SetTrigger(_angerAnimHash);
                    break;
                case AttackState.FinishAttack:
                    _animator.SetTrigger(_defaultAnimHash);
                    break;
            }
        }
    }
}