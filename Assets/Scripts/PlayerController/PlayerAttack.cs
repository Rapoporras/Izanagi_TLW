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
        [SerializeField] private PlayerAbilitiesData _abilitiesData;

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
        private AttackInfo _lastAttackInfo;
        public AttackInfo LastAttackInfo => _lastAttackInfo;
        
        private List<Collider2D> _overlappedColliders = new List<Collider2D>();
        private ContactFilter2D _contactFilterEnemies;
        private ContactFilter2D _contactFilterBreakableWall;

        public bool attackInput;
        public bool wallAttackInput;

        public float DamageMultiplier { get; set; } = 1f;
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
            
            _contactFilterEnemies = new ContactFilter2D();
            _contactFilterEnemies.SetLayerMask(_hurtboxLayer);
            _contactFilterEnemies.useTriggers = true;

            _contactFilterBreakableWall = new ContactFilter2D();
            _contactFilterEnemies.SetLayerMask(_breakableWallLayer);
            
            attackInput = false;
            wallAttackInput = false;
        }

        private void OnEnable()
        {
            InputManager.Instance.PlayerActions.Attack.started += Attack;
            InputManager.Instance.PlayerActions.WallBreaking.started += WallAttack;
            
            _movementAction = InputManager.Instance.PlayerActions.Movement;
        }

        private void OnDisable()
        {
            InputManager.Instance.PlayerActions.Attack.started -= Attack;
            InputManager.Instance.PlayerActions.WallBreaking.started -= WallAttack;
        }
        #endregion
        
        #region STATE MACHINE
        protected override void SetStates()
        {
            States.Add(PlayerAttackStates.NotAttacking, new PlayerNotAttackingState(PlayerAttackStates.NotAttacking, this));
            States.Add(PlayerAttackStates.AttackEntry, new PlayerAttackEntryState(PlayerAttackStates.AttackEntry, this));
            States.Add(PlayerAttackStates.AttackCombo, new PlayerAttackComboState(PlayerAttackStates.AttackCombo, this));
            States.Add(PlayerAttackStates.AttackFinisher, new PlayerAttackFinisherState(PlayerAttackStates.AttackFinisher, this));
            States.Add(PlayerAttackStates.WallAttack, new PlayerWallAttackState(PlayerAttackStates.WallAttack, this));

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
            Physics2D.OverlapCollider(_attackArea, _contactFilterEnemies, _overlappedColliders);
            foreach (var entity in _overlappedColliders)
            {
                if (entity.transform.parent.TryGetComponent(out EntityHealth entityHealth) && !entity.CompareTag("Player"))
                {
                    if (!entityHealth.IsInvulnerable && entityHealth.damageable)
                    {
                        int damage = Mathf.CeilToInt(AttackData.attackDamage * DamageMultiplier);
                        entityHealth.Damage(damage, true);
                        UpdateMannaPoints(_mannaPerAttack);
                    }
                    
                    if (!_hasCollided)
                    {
                        _hasCollided = true;
                        ApplyAttackForces(entityHealth);
                    }
                }
            }
        }

        public void StopAttack()
        {
            attackInput = false;
            _hasCollided = false;
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
        
        #region WALL BREAKING
        private void WallAttack(InputAction.CallbackContext context)
        {
            if (!_abilitiesData.breakWalls) return;
            if (IsAttacking) return;
            
            if (context.ReadValueAsButton())
            {
                wallAttackInput = true;
            }
        }

        public void CheckBreakableWalls()
        {
            _overlappedColliders.Clear();
            Physics2D.OverlapCollider(_attackArea, _contactFilterBreakableWall, _overlappedColliders);
            foreach (var wall in _overlappedColliders)
            {
                if (wall.CompareTag("BreakableWall") && wall.TryGetComponent(out BreakableWall breakableWall))
                {
                    if (!_hasCollided)
                    {
                        _hasCollided = true;
                        breakableWall.ApplyHit();
                    }
                }
            }
        }
        
        public void StopWallAttack()
        {
            wallAttackInput = false;
            _hasCollided = false;
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

        public void SetWallAttackAnimation()
        {
            _playerAnimations.SetWallAttackAnimation();
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