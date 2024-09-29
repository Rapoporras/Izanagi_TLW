using System.Collections.Generic;
using GlobalVariables;
using Health;
using PlayerController.Data;
using PlayerController.States;
using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    public class PlayerAttack : BaseStateMachine<PlayerAttackStates>
    {
        [field: Header("Data")]
        [field: SerializeField] public PlayerAttackData AttackData { get; private set; }
        // [SerializeField] private PlayerAbilitiesData _abilitiesData;

        [Header("Manna")]
        [SerializeField] private IntReference _currentManna;
        [SerializeField] private IntReference _maxManna;
        [SerializeField] private int _mannaPerAttack;
        
        [Space(10), Header("Settings")]
        [SerializeField] private LayerMask _hurtboxLayer;
        [SerializeField] private LayerMask _breakableWallLayer;
        [SerializeField] private Collider2D _attackArea;
        
        private PlayerAnimations _playerAnimations;
        private PlayerMovement _playerMovement;
        
        private InputAction _movementAction;

        private bool _hasCollided;
        private bool _breakableWallHit;
        private AttackInfo _lastAttackInfo;
        public AttackInfo LastAttackInfo => _lastAttackInfo;
        
        private List<Collider2D> _overlappedColliders = new List<Collider2D>();
        private ContactFilter2D _attackContactFilter;

        [HideInInspector] public bool attackInput;

        public float AttackAirBoost { get; set; } = 1f;
        public bool IsGrounded => _playerMovement.IsGrounded;
        private bool IsAttacking => _currentState.StateKey != PlayerAttackStates.NotAttacking;
        private bool AttackWindowActive => _playerAnimations.AttackWindowActive;
        
        public enum AttackType
        {
            Horizontal, Downwards, Upwards
        }
        
        public struct AttackInfo
        {
            public AttackType Type;
            public Vector2 Direction;
        }

        #region UNITY METHODS
        private void Awake()
        {
            _playerAnimations = GetComponent<PlayerAnimations>();
            _playerMovement = GetComponent<PlayerMovement>();
            
            _attackContactFilter = new ContactFilter2D();
            _attackContactFilter.SetLayerMask(_hurtboxLayer | _breakableWallLayer);
            _attackContactFilter.useTriggers = true;
            
            attackInput = false;
        }

        private void OnEnable()
        {
            InputManager.Instance.PlayerActions.Attack.started += Attack;
            
            _movementAction = InputManager.Instance.PlayerActions.Movement;
        }

        private void OnDisable()
        {
            InputManager.Instance.PlayerActions.Attack.started -= Attack;
        }
        #endregion
        
        #region STATE MACHINE
        protected override void SetStates()
        {
            States.Add(PlayerAttackStates.NotAttacking, new PlayerNotAttackingState(PlayerAttackStates.NotAttacking, this));
            States.Add(PlayerAttackStates.AttackEntry, new PlayerAttackEntryState(PlayerAttackStates.AttackEntry, this));
            States.Add(PlayerAttackStates.AttackCombo, new PlayerAttackComboState(PlayerAttackStates.AttackCombo, this));
            States.Add(PlayerAttackStates.AttackFinisher, new PlayerAttackFinisherState(PlayerAttackStates.AttackFinisher, this));
            
            _currentState = States[PlayerAttackStates.NotAttacking];
        }
        #endregion

        #region ATTACK
        private void Attack(InputAction.CallbackContext context)
        {
            _lastAttackInfo.Type = GetAttackType();
            _lastAttackInfo.Direction = GetAttackDirection(_lastAttackInfo.Type);
            
            if (context.ReadValueAsButton() && AttackWindowActive)
            {
                if (_lastAttackInfo.Type == AttackType.Downwards && IsGrounded)
                    return;
                
                attackInput = true;
            }
        }

        public void ApplyDamage()
        {
            _overlappedColliders.Clear();
            Physics2D.OverlapCollider(_attackArea, _attackContactFilter, _overlappedColliders);
            foreach (var entity in _overlappedColliders)
            {
                if (entity.transform.parent // check if game object has a parent
                    && entity.transform.parent.TryGetComponent(out EntityHealth entityHealth)
                    && !entity.CompareTag("Player"))
                {
                    if (!entityHealth.IsInvulnerable && entityHealth.damageable)
                    {
                        int damage = Mathf.CeilToInt(AttackData.AttackDamage * AttackAirBoost);
                        entityHealth.Damage(damage, true);
                        UpdateMannaPoints(_mannaPerAttack);
                    }
                    
                    if (!_hasCollided)
                    {
                        _hasCollided = true;
                        ApplyAttackForces(entityHealth);
                    }
                }
                
                if (entity.CompareTag("BreakableWall") && entity.TryGetComponent(out BreakableWall breakableWall))
                {
                    if (!_breakableWallHit)
                    {
                        breakableWall.ApplyHit();
                        _breakableWallHit = true;
                    }
                }
            }
        }

        public void StopAttack()
        {
            attackInput = false;
            _hasCollided = false;
            _breakableWallHit = false;
        }

        private void ApplyAttackForces(EntityHealth entityHealth)
        { 
            if (_lastAttackInfo.Type == AttackType.Downwards
                && entityHealth.giveUpwardForce
                && !IsGrounded)
            {
                _playerMovement.ApplyPogoJump();
            }
            else if (_lastAttackInfo.Type == AttackType.Horizontal)
            {
                _playerMovement.ApplyRecoil(_lastAttackInfo.Direction * -1f);
            }
        }
        #endregion
        
        #region MANNA
        private void UpdateMannaPoints(int amount)
        {
            _currentManna.Value = Mathf.Clamp(_currentManna + amount, 0, _maxManna);
        }
        #endregion
        
        #region UTILS
        public void SetAttackAnimation()
        {
            _playerAnimations.SetAttackAnimation(
                (int) _lastAttackInfo.Type, (int) _currentState.StateKey);
        }

        public void ActivateAttackWindow()
        {
            _playerAnimations.AttackWindowActive = true;
        }
        
        private AttackType GetAttackType()
        {
            Vector2 direction = _movementAction.ReadValue<Vector2>().normalized;
            float angle = Mathf.Abs(Mathf.Asin(direction.y) * Mathf.Rad2Deg);

            AttackType attackType = AttackType.Horizontal;

            if (angle >= AttackData.angleDirectionOffset)
            {
                if (direction.y > 0)
                    attackType = AttackType.Upwards;
                else if (!IsGrounded)
                    attackType = AttackType.Downwards;
            }
            
            return attackType;
        }

        private Vector2 GetAttackDirection(AttackType type)
        {
            Vector2 dir = type switch
            {
                AttackType.Downwards => Vector2.down,
                AttackType.Upwards => Vector2.up,
                AttackType.Horizontal => _playerMovement.IsFacingRight ? Vector2.right : Vector2.left,
                _ => Vector2.zero
            };

            return dir;
        }
        #endregion

        #region DEBUG
#if UNITY_EDITOR
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 150, 500, 50));
            string rootStateName = _currentState.Name;
            GUILayout.Label($"<color=black><size=50>State: {rootStateName}</size></color>");
            GUILayout.EndArea();
        }
#endif
        #endregion
    }
}