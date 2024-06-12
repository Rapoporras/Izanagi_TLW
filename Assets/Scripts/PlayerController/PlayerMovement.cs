﻿using System.Collections;
using PlayerController.States;
using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    [RequireComponent(typeof(Rigidbody2D), typeof(RaycastInfo))]
    public class PlayerMovement : BaseStateMachine<PlayerStates>
    {
        
        #region Serialized Fields
        [field: Header("Dependencies")]
        [field: SerializeField] public PlayerMovementData Data { get; private set; }
        [SerializeField] private float _xInputDeadZone = 0.25f;
        #endregion
        
        #region Private variables
        private Rigidbody2D _rb2d;
        private RaycastInfo _raycastInfo;

        private PlayerInputActions _playerInputActions;
        private InputAction _movementAction;

        private bool _inKnockBack;
        private float _lastKnockBackSpeed;
        #endregion

        public PlayerStates CurrentState => _currentState.StateKey;
        
        #region Dash Parameters
        private float _lastPressedDashTime;
        private bool _isDashRefilling;
        public bool IsDashActive { get; set; } // set to true when grounded, and false when dashing
        public bool CanDash => IsDashActive && !_isDashRefilling;
        public bool DashRequest { get; private set; }
        #endregion
        
        #region Movement Parameters

        public Vector2 MovementDirection
        {
            get
            {
                Vector2 direction = _movementAction.ReadValue<Vector2>();
                if (Mathf.Abs(direction.x) < _xInputDeadZone)
                    direction.x = 0;
                return direction;
            }
        }
        public bool LeftWallHit => _raycastInfo.HitInfo.Left;
        public bool RightWallHit => _raycastInfo.HitInfo.Right;
        public Vector2 Velocity
        {
            get => _rb2d.velocity;
            set => _rb2d.velocity = value;
        }
        public bool IsFacingRight { get; private set; }
        #endregion
        
        #region Jump Parameters
        private float _lastPressedJumpTime;
        private int _additionalJumps;
        public bool IsGrounded => _raycastInfo.HitInfo.Below;
        public bool IsWallSliding => (LeftWallHit || RightWallHit) && !IsGrounded;
        public bool JumpRequest { get; private set; }
        public bool HandleLongJumps { get; private set; }
        public bool IsActiveCoyoteTime { get; set; }
        public int AdditionalJumpsAvailable
        {
            get => _additionalJumps;
            set => _additionalJumps = Mathf.Clamp(value, 0, Data.additionalJumps);
        }
        #endregion
        
        #region KonckBack Parameters
        public bool IsTakingDamage { get; set; }
        public bool UseKnockBackAccelInAir { get; set; }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _rb2d.gravityScale = 0f;
            _raycastInfo = GetComponent<RaycastInfo>();

            _playerInputActions = new PlayerInputActions();
        }

        protected override void Start()
        {
            base.Start();
            SetGravityScale(Data.gravityScale);

            IsFacingRight = true;
        }

        protected override void Update()
        {
            base.Update();
            
            ManageJumpBuffer();
            ManageDashBuffer();
            
            if (MovementDirection.x != 0 && _currentState.StateKey != PlayerStates.Dashing)
                SetDirectionToFace(MovementDirection.x > 0);
        }

        private void OnEnable()
        {
            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }
        #endregion

        #region State Machine Functions
        protected override void SetStates()
        {
            States.Add(PlayerStates.Grounded, new PlayerGroundedState(PlayerStates.Grounded, this));
            States.Add(PlayerStates.Jumping, new PlayerJumpingState(PlayerStates.Jumping, this));
            States.Add(PlayerStates.Falling, new PlayerFallingState(PlayerStates.Falling, this));
            States.Add(PlayerStates.WallSliding, new PlayerWallSlidingState(PlayerStates.WallSliding, this));
            States.Add(PlayerStates.WallJumping, new PlayerWallJumpingState(PlayerStates.WallJumping, this));
            States.Add(PlayerStates.Dashing, new PlayerDashingState(PlayerStates.Dashing, this));
            States.Add(PlayerStates.Damaged, new PlayerDamagedState(PlayerStates.Damaged, this));
            
            _currentState = States[PlayerStates.Grounded];
        }
        #endregion
        
        #region Input
        private void EnableInput()
        {
            _movementAction = _playerInputActions.Player.Movement;
            _movementAction.Enable();

            _playerInputActions.Player.Jump.started += OnJumpAction;
            _playerInputActions.Player.Jump.canceled += OnJumpAction;
            _playerInputActions.Player.Jump.Enable();

            _playerInputActions.Player.Dash.performed += OnDashAction;
            _playerInputActions.Player.Dash.Enable();
        }

        private void DisableInput()
        {
            _movementAction.Disable();
            _playerInputActions.Player.Jump.Disable();
            _playerInputActions.Player.Dash.Disable();
        }
        #endregion
        
        #region Movement Functions
        public void Run(float lerpAmount, bool canAddBonusJumpApex)
        {
            float targetSpeed = GetTargetSpeed();
            // smooths change
            targetSpeed = Mathf.Lerp(_rb2d.velocity.x, targetSpeed, lerpAmount);

            float accelRate;
            if (IsGrounded)
            {
                accelRate = Mathf.Abs(targetSpeed) > 0.01f
                    ? Data.runAccelAmount
                    : Data.runDecelAmount;
            }
            else
            {
                float accel = UseKnockBackAccelInAir ? Data.kbAccelInArMult : Data.accelInAirMult;
                float decel = UseKnockBackAccelInAir ? Data.kbDecelInArMult : Data.decelInAirMult;
                
                accelRate = Mathf.Abs(targetSpeed) > 0.01f
                    ? Data.runAccelAmount * accel
                    : Data.runDecelAmount * decel;
            }
            
            if (canAddBonusJumpApex && Mathf.Abs(_rb2d.velocity.y) < Data.jumpHangTimeThreshold)
            {
                // makes the jump feels a bit more bouncy, responsive and natural
                accelRate *= Data.jumpHangAcceleration;
                targetSpeed *= Data.jumpHangMaxSpeedMult;
            }
            
            float speedDif = targetSpeed - _rb2d.velocity.x;
            float movement = speedDif * accelRate;
            
            _rb2d.AddForce(movement * Vector2.right, ForceMode2D.Force);
            // same as:
            // _rb2d.velocity = new Vector2(
            //      _rb2d.velocity.x + (Time.fixedDeltaTime * speedDif * accelRate) / _rb2d.mass,
            //      _rb2d.velocity.y);

            if (speedDif == 0)
                UseKnockBackAccelInAir = false;
        }

        public void Slide()
        {
            // remove the remaining upwards impulse
            if (_rb2d.velocity.y > 0)
                _rb2d.AddForce(-_rb2d.velocity.y * Vector2.up, ForceMode2D.Impulse);

            float speedDif = Data.slideSpeed - _rb2d.velocity.y;
            float movement = speedDif * Data.slideAccel;
            
            //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
            movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif)  * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));
            
            _rb2d.AddForce(movement * Vector2.up);
        }

        private float GetTargetSpeed()
        {
            float targetSpeed = MovementDirection.x * Data.runMaxSpeed;
            if (_inKnockBack)
                targetSpeed = _lastKnockBackSpeed;
            return targetSpeed;
        }
        #endregion

        #region Jump Functions
        public void Jump()
        {
            JumpRequest = false;
            
            float force = Data.jumpForce;
            
            // avoid shorter jumps when falling and jumping with coyote time
            if (_rb2d.velocity.y < 0)
                force -= _rb2d.velocity.y;
            
            _rb2d.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        public void WallJump(int dir)
        {
            Vector2 force = Data.wallJumpForce;
            force.x *= dir;

            if (Mathf.Sign(_rb2d.velocity.x) != Mathf.Sign(force.x))
                force.x -= _rb2d.velocity.x;

            if (_rb2d.velocity.y < 0)
                force.y -= _rb2d.velocity.y;
            
            _rb2d.AddForce(force, ForceMode2D.Impulse);
        }

        public void ResetAdditionalJumps()
        {
            AdditionalJumpsAvailable = Data.additionalJumps;
        }
        
        private void OnJumpAction(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton())
            {
                JumpRequest = true;
                _lastPressedJumpTime = Data.jumpInputBufferTime;
            }
            
            HandleLongJumps = context.ReadValueAsButton();
        }

        private void ManageJumpBuffer()
        {
            if (!JumpRequest) return;
            
            _lastPressedJumpTime -= Time.deltaTime;
            if (_lastPressedJumpTime <= 0)
            {
                JumpRequest = false;
            }
        }
        #endregion
        
        #region Wall Sliding Functions
        public bool CanWallSlide()
        {
            if ((LeftWallHit || RightWallHit) && MovementDirection != Vector2.zero)
                return true;
            
            return false;
        }
        #endregion
        
        #region Dash Functions
        public void RefillDash()
        {
            StartCoroutine(nameof(PerformRefillDash));
        }
        
        private IEnumerator PerformRefillDash()
        {
            _isDashRefilling = true;
            yield return new WaitForSeconds(Data.dashRefillTime);
            _isDashRefilling = false;
        }
        
        private void OnDashAction(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton())
            {
                DashRequest = true;
                _lastPressedDashTime = Data.dashInputBufferTime;
            }
        }

        private void ManageDashBuffer()
        {
            if (!DashRequest) return;
            
            _lastPressedDashTime -= Time.deltaTime;
            if (_lastPressedDashTime <= 0)
            {
                DashRequest = false;
            }
        }
        #endregion
        
        #region Attack Movement Functions
        public void ApplyRecoil(Vector2 direction)
        {
            StartCoroutine(SetTemporalMaxSpeed(Data.recoilSpeed * direction.x, Data.recoilDuration));
            _rb2d.AddForce(Data.recoilSpeed * 50 * direction, ForceMode2D.Force);
        }
        
        public void ApplyPogoJump()
        {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, 0f);
            _rb2d.AddForce(Vector2.up * Data.pogoForce, ForceMode2D.Impulse);
        }

        public void ApplyDamageKnockBack(int xDirection) // xDirection: -1, 1
        {
            IsTakingDamage = true;
            
            Vector2 velocity = Data.knockBackVelocity;
            velocity.x *= xDirection;
            
            StartCoroutine(SetTemporalMaxSpeed(velocity.x, Data.knockBackDuration));
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, 0f);
            _rb2d.AddForce(velocity * 50, ForceMode2D.Force);
        }

        private IEnumerator SetTemporalMaxSpeed(float speed, float duration)
        {
            _lastKnockBackSpeed = speed;
            _inKnockBack = true;
            yield return new WaitForSeconds(duration);
            _inKnockBack = false;
        }
        #endregion
        
        #region General Methods
        public void SetGravityScale(float scale)
        {
            _rb2d.gravityScale = scale;
        }

        public void Sleep(float duration)
        {
            StartCoroutine(nameof(PerformSleep), duration);
        }

        private IEnumerator PerformSleep(float duration)
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1;
        }
        
        public void SetDirectionToFace(bool isMovingRight)
        {
            if (isMovingRight != IsFacingRight)
            {
                // IsFacingRight = isMovingRight;
                IsFacingRight = !IsFacingRight;
                transform.rotation = Quaternion.Euler(
                    0f,
                    IsFacingRight ? 0 : 180f,
                    0f);
            }
        }
        #endregion
        
        #region Debug
        #if UNITY_EDITOR
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            string rootStateName = _currentState.Name;
            GUILayout.Label($"<color=black><size=50>State: {rootStateName}</size></color>");
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label($"<color=black><size=30>Input: {MovementDirection}</size></color>");
            GUILayout.EndHorizontal();
        }
        #endif
        #endregion
    }
}