using PlayerController.States;
using UnityEngine;

namespace PlayerController
{
    public class PlayerAnimations : MonoBehaviour
    {
        private PlayerMovement _player;
        
        private Animator _animator;
        private int _xSpeedHash;
        private int _ySpeedHash;
        private int _isGroundedHash;
        private int _isSlidingHash;
        private int _isDashingHash;
        private int _isTakingDamageHash;
        
        private int _attackHash;
        private int _attackTypeHash;
        private int _wallAttackHash;
        
        private void Awake()
        {
            _player = GetComponent<PlayerMovement>();
            
            _animator = GetComponent<Animator>();
            _xSpeedHash = Animator.StringToHash("xSpeed");
            _ySpeedHash = Animator.StringToHash("ySpeed");
            _isGroundedHash = Animator.StringToHash("isGrounded");
            _isSlidingHash = Animator.StringToHash("isSliding");
            _isDashingHash = Animator.StringToHash("isDashing");
            _isTakingDamageHash = Animator.StringToHash("isTakingDamage");
            
            _attackHash = Animator.StringToHash("attack");
            _attackTypeHash = Animator.StringToHash("attackType");
            _wallAttackHash = Animator.StringToHash("wallAttack");
        }

        private void Update()
        {
            if (_player.CanDash && _player.DashRequest)
                _animator.SetTrigger(_isDashingHash);
        }

        private void LateUpdate()
        {
            _animator.SetFloat(_xSpeedHash, Mathf.Abs(_player.Velocity.x));
            _animator.SetFloat(_ySpeedHash, _player.Velocity.y);
            _animator.SetBool(_isGroundedHash, _player.IsGrounded);
            _animator.SetBool(_isSlidingHash, _player.IsWallSliding);
            _animator.SetBool(_isDashingHash, _player.CurrentState == PlayerStates.Dashing);
            _animator.SetBool(_isTakingDamageHash, _player.IsTakingDamage);
        }

        public void SetAttackAnimation(int attackType)
        {
            _animator.SetInteger(_attackTypeHash, attackType);
            _animator.SetTrigger(_attackHash);
        }

        public void SetWallAttackAnimation()
        {
            _animator.SetTrigger(_wallAttackHash);
        }
    }
}
