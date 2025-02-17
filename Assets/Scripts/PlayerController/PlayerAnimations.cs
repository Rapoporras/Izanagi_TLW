﻿using PlayerController.States;
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
        
        private int _isDeadHash;
        private int _deathHash;
        
        private int _recoverHealthHash;
        
        private int _attackHash;
        private int _attackTypeHash;
        private int _attackComboHash;

        private int _attackWindowActiveHash;

        //Variable con valor random para usarse en los blendTrees
        private int _animationIddleSelect;

        public bool AttackWindowActive
        {
            get => _animator.GetBool(_attackWindowActiveHash);
            set => _animator.SetBool(_attackWindowActiveHash, value);
        }
        
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
            
            _isDeadHash = Animator.StringToHash("isDead");
            _deathHash = Animator.StringToHash("death");

            _recoverHealthHash = Animator.StringToHash("recoverHealth");

            
            _attackHash = Animator.StringToHash("attack");
            _attackTypeHash = Animator.StringToHash("attackType");
            _attackComboHash = Animator.StringToHash("attackCombo");

            _attackWindowActiveHash = Animator.StringToHash("attackWindowActive");

            _animationIddleSelect = Animator.StringToHash("animationSelect");
        }

        // private void Update()
        // {
        //     if (_player.CanDash && _player.DashRequest)
        //         _animator.SetTrigger(_isDashingHash);
        // }

        private void LateUpdate()
        {
            _animator.SetFloat(_xSpeedHash, Mathf.Abs(_player.Velocity.x));
            _animator.SetFloat(_ySpeedHash, _player.Velocity.y);
            _animator.SetBool(_isGroundedHash, _player.IsGrounded);
            _animator.SetBool(_isSlidingHash, _player.IsWallSliding);
            _animator.SetBool(_isDashingHash, _player.CurrentState == PlayerStates.Dashing);
            _animator.SetBool(_isTakingDamageHash, _player.IsTakingDamage);
        }

        #region PLAY ANIMATIONS

        //Llamado desde evento de fin de animación de personaje Iddle
        private void UpdateAnimationIddleSelect()
        {
           _animator.SetFloat(_animationIddleSelect, Random.Range(0f, 1f));
        }

        public void SetAttackAnimation(int attackType, int attackCombo)
        {
            _animator.SetInteger(_attackTypeHash, attackType);
            _animator.SetInteger(_attackComboHash, attackCombo);
            _animator.SetTrigger(_attackHash);
        }

        public void PlayRecoverHealthAnimation()
        {
            _animator.SetTrigger(_recoverHealthHash);
        }

        public void ActivateAttackWindow() // called in animation keyframe
        {
            _animator.SetBool(_attackWindowActiveHash, true);
        }
        
        public void DeactivateAttackWindow() // called in animation keyframe
        {
            _animator.SetBool(_attackWindowActiveHash, false);
        }

        public void SetIsDeadVariable(bool isDead)
        {
            _animator.SetBool(_isDeadHash, isDead);
            if (isDead)
                _animator.SetTrigger(_deathHash);
        }
        #endregion
    }
}
