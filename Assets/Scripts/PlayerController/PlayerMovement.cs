using System.Collections;
using CameraSystem;
using GlobalVariables;
using PlayerController.Data;
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
        [field: Header("Data")]
        [field: SerializeField] public PlayerMovementData MovementData { get; private set; }
        [field: SerializeField] public PlayerAbilitiesData AbilitiesData { get; private set; }
        
        [Space(10), Header("Settings")]
        [SerializeField] private float _xInputDeadZone = 0.25f;

        [Space(10)] public GameObject wallImpulseArrow;

        [Space(10)]
        public BoolReference dashInvulnerability;

        [Space(10)]
        [SerializeField] private CameraFollowObject _cameraFollowObject;
        #endregion
        
        #region Private variables
        private Rigidbody2D _rb2d;
        private RaycastInfo _raycastInfo;

        private InputAction _movementAction;

        private bool _inKnockBack;
        private float _lastKnockBackSpeed;

        private float _fallSpeedYDampingChangeThreshold;
        #endregion

        public PlayerStates CurrentState => _currentState == null ? PlayerStates.Grounded : _currentState.StateKey;
        public bool HandleWallImpulse { get; private set; }
        public Transform CameraTarget => _cameraFollowObject.transform;
        // public Transform CameraTarget => transform;
        
        #region Dash Properties
        private float _lastPressedDashTime;
        private bool _isDashRefilling;
        public bool IsDashActive { get; set; } // set to true when grounded, and false when dashing
        public bool CanDash => IsDashActive && !_isDashRefilling;
        public bool DashRequest { get; private set; }
        #endregion
        
        #region Movement Properties

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
        public bool IsFacingRight { get; private set; } = true;
        #endregion
        
        #region Jump Properties
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
            set => _additionalJumps = Mathf.Clamp(value, 0, MovementData.additionalJumps);
        }
        #endregion
        
        #region KonckBack Properties
        public bool IsTakingDamage { get; set; }
        public bool UseKnockBackAccelInAir { get; set; }
        #endregion

        #region Unity Functions
        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _rb2d.gravityScale = 0f;
            _raycastInfo = GetComponent<RaycastInfo>();
        }

        protected override void Start()
        {
            base.Start();
            SetGravityScale(MovementData.gravityScale);
            
            _fallSpeedYDampingChangeThreshold = CameraManager.Instance.fallSpeedYDampingChangeThreshold;
        }

        protected override void Update()
        {
            base.Update();
            
            ManageJumpBuffer();
            ManageDashBuffer();

            if (MovementDirection.x != 0
                && _currentState.StateKey != PlayerStates.Dashing
                && _currentState.StateKey != PlayerStates.WallImpulse)
            {
                SetDirectionToFace(MovementDirection.x > 0);
            }
            
            HandleWallImpulse = InputManager.Instance.PlayerActions.WallImpulse.IsPressed();

            if (_rb2d.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.Instance.IsLerpingYDamping
                && !CameraManager.Instance.LerpedFromPlayerFalling)
            {
                CameraManager.Instance.LerpYDamping(true);
            }
            if (_rb2d.velocity.y >= 0f && !CameraManager.Instance.IsLerpingYDamping
                && CameraManager.Instance.LerpedFromPlayerFalling)
            {
                CameraManager.Instance.LerpedFromPlayerFalling = false;
                CameraManager.Instance.LerpYDamping(false);
            }
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
            States.Add(PlayerStates.WallImpulse, new PlayerWallImpulseState(PlayerStates.WallImpulse, this));
            
            _currentState = States[PlayerStates.Grounded];
        }
        #endregion
        
        #region Input
        private void EnableInput()
        {
            _movementAction = InputManager.Instance.PlayerActions.Movement;
            
            InputManager.Instance.PlayerActions.Jump.started += OnJumpAction;
            InputManager.Instance.PlayerActions.Jump.canceled += OnJumpAction;
            
            InputManager.Instance.PlayerActions.Dash.performed += OnDashAction;
        }

        private void DisableInput()
        {
            InputManager.Instance.PlayerActions.Jump.started -= OnJumpAction;
            InputManager.Instance.PlayerActions.Jump.canceled -= OnJumpAction;
            
            InputManager.Instance.PlayerActions.Dash.performed -= OnDashAction;
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
                    ? MovementData.runAccelAmount
                    : MovementData.runDecelAmount;
            }
            else
            {
                float accel = UseKnockBackAccelInAir ? MovementData.kbAccelInArMult : MovementData.accelInAirMult;
                float decel = UseKnockBackAccelInAir ? MovementData.kbDecelInArMult : MovementData.decelInAirMult;
                
                accelRate = Mathf.Abs(targetSpeed) > 0.01f
                    ? MovementData.runAccelAmount * accel
                    : MovementData.runDecelAmount * decel;
            }
            
            if (canAddBonusJumpApex && Mathf.Abs(_rb2d.velocity.y) < MovementData.jumpHangTimeThreshold)
            {
                // makes the jump feels a bit more bouncy, responsive and natural
                accelRate *= MovementData.jumpHangAcceleration;
                targetSpeed *= MovementData.jumpHangMaxSpeedMult;
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

            float speedDif = MovementData.slideSpeed - _rb2d.velocity.y;
            float movement = speedDif * MovementData.slideAccel;
            
            //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
            movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif)  * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));
            
            _rb2d.AddForce(movement * Vector2.up);
        }

        private float GetTargetSpeed()
        {
            float targetSpeed = MovementDirection.x * MovementData.runMaxSpeed;
            if (_inKnockBack)
                targetSpeed = _lastKnockBackSpeed;
            return targetSpeed;
        }
        #endregion

        #region Jump Functions
        public void Jump()
        {
            JumpRequest = false;
            
            float force = MovementData.jumpForce;
            
            // avoid shorter jumps when falling and jumping with coyote time
            if (_rb2d.velocity.y < 0)
                force -= _rb2d.velocity.y;
            
            _rb2d.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        public void WallJump(int dir)
        {
            Vector2 force = MovementData.wallJumpForce;
            force.x *= dir;

            if (Mathf.Sign(_rb2d.velocity.x) != Mathf.Sign(force.x))
                force.x -= _rb2d.velocity.x;

            if (_rb2d.velocity.y < 0)
                force.y -= _rb2d.velocity.y;
            
            _rb2d.AddForce(force, ForceMode2D.Impulse);
        }

        public void ResetAdditionalJumps()
        {
            AdditionalJumpsAvailable = MovementData.additionalJumps;
        }

        public bool CanPerformExtraJump()
        {
            return AdditionalJumpsAvailable > 0 && AbilitiesData.doubleJump;
        }
        
        private void OnJumpAction(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton())
            {
                JumpRequest = true;
                _lastPressedJumpTime = MovementData.jumpInputBufferTime;
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
            yield return new WaitForSeconds(MovementData.dashRefillTime);
            _isDashRefilling = false;
        }
        
        private void OnDashAction(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton())
            {
                DashRequest = true;
                _lastPressedDashTime = MovementData.dashInputBufferTime;
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
            StartCoroutine(SetTemporalMaxSpeed(MovementData.recoilSpeed * direction.x, MovementData.recoilDuration));
            _rb2d.AddForce(MovementData.recoilSpeed * 50 * direction, ForceMode2D.Force);
        }
        
        public void ApplyPogoJump()
        {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, 0f);
            _rb2d.AddForce(Vector2.up * MovementData.pogoForce, ForceMode2D.Impulse);
            
            ResetAdditionalJumps();
            IsDashActive = true;
        }

        public void ApplyDamageKnockBack(int xDirection) // xDirection: -1, 1
        {
            IsTakingDamage = true;
            
            Vector2 velocity = MovementData.knockBackVelocity;
            velocity.x *= xDirection;
            
            StartCoroutine(SetTemporalMaxSpeed(velocity.x, MovementData.knockBackDuration));
            _rb2d.velocity = new Vector2(0f, 0f);
            _rb2d.AddForce(velocity, ForceMode2D.Impulse);
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

        public void SetDirectionToFace(bool isMovingRight)
        {
            if (isMovingRight != IsFacingRight)
            {
                IsFacingRight = !IsFacingRight;
                transform.rotation = Quaternion.Euler(
                    0f,
                    IsFacingRight ? 0 : 180f,
                    0f);
                
                _cameraFollowObject.CallTurn();
            }
        }

        public void SetCameraFollowObject()
        {
            _cameraFollowObject.Initialize(_rb2d, IsFacingRight);
            _cameraFollowObject.transform.parent = transform.parent;
        }
        #endregion
        
        #region Debug
        #if UNITY_EDITOR
        // private void OnGUI()
        // {
        //     GUILayout.BeginArea(new Rect(10, 10, 500, 200));
        //     string rootStateName = _currentState.Name;
        //     GUILayout.Label($"<color=black><size=50>State: {rootStateName}</size></color>");
        //     
        //     GUILayout.Label($"<color=black><size=30>Input: {MovementDirection}</size></color>");
        //     GUILayout.EndArea();
        // }
        #endif
        #endregion
    }
}