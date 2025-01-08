using UnityEngine;

namespace Bosses
{
    public class SeiryuBody : MonoBehaviour, ISeiryuAttackStateHandler
    {
        private Animator _animator;

        private int _defaultAnimHash;
        private int _attackLeftAnimHash;
        private int _attackRightAnimHash;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _defaultAnimHash = Animator.StringToHash("default");
            _attackLeftAnimHash = Animator.StringToHash("attack_left");
            _attackRightAnimHash = Animator.StringToHash("attack_right");
        }

        public void OnAttackStateChange(SeiryuAttackInfo info)
        {
            switch (info.state)
            {
                case AttackState.StartAttack:
                    if (info.side == ClawSide.Left)
                        _animator.SetTrigger(_attackLeftAnimHash);
                    else if (info.side == ClawSide.Right)
                        _animator.SetTrigger(_attackRightAnimHash);
                    break;
                case AttackState.FinishAttack:
                    _animator.SetTrigger(_defaultAnimHash);
                    break;
            }
        }
    }
}
