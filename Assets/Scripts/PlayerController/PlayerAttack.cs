using System.Collections;
using GlobalVariables;
using Health;
using PlayerController.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace PlayerController
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private PlayerAttackData _attackData;
        [SerializeField] private PlayerAbilitiesData _abilitiesData;

        [Header("Manna")]
        [SerializeField] private IntReference _currentManna;
        [SerializeField] private IntReference _maxManna;
        [SerializeField] private int _mannaPerAttack;
        
        [Space(10), Header("Settings")]
        [SerializeField] private Transform _horizontalAttackPoint;
        [SerializeField] private Transform _upperAttackPoint;
        [SerializeField] private Transform _bottomAttackPoint;
        [SerializeField] private float _attackRadius;
        [SerializeField] private LayerMask _hurtboxLayer;
        
        private PlayerAnimations _playerAnimations;
        private PlayerMovement _playerMovement;
        
        private InputAction _movementAction;

        private bool _hasCollided;
        private AttackInfo _lastAttackInfo;
        
        private bool _isPlayerAttacking;
        private Timer _attackTimer;

        private bool _isPlayerWallAttacking;
        private Timer _wallAttackTimer;

        public float DamageMultiplier { get; set; } = 1f;
        public bool IsAttacking => _isPlayerAttacking || _isPlayerWallAttacking;

        private enum AttackType
        {
            Horizontal, Downwards, Upwards
        }
        
        struct AttackInfo
        {
            public AttackType Type;
            public Vector2 Direction;
        }

        private void Awake()
        {
            _playerAnimations = GetComponent<PlayerAnimations>();
            _playerMovement = GetComponent<PlayerMovement>();
            
            _isPlayerAttacking = false;
            _isPlayerWallAttacking = false;

            _attackTimer = new Timer(_attackData.attackDuration);
            _wallAttackTimer = new Timer(_attackData.wallAttackDuration);
        }

        private void Update()
        {
            
            if (_isPlayerAttacking)
            {
                CheckAttackCollisions();
                _attackTimer.Tick(Time.deltaTime);
            }

            if (_isPlayerWallAttacking)
            {
                CheckBreakableWalls();
                _wallAttackTimer.Tick(Time.deltaTime);
            }
        }

        private void OnEnable()
        {
            InputManager.Instance.PlayerActions.Attack.started += Attack;
            InputManager.Instance.PlayerActions.WallBreaking.started += WallAttack;
            
            _movementAction = InputManager.Instance.PlayerActions.Movement;

            _attackTimer.OnTimerEnd += StopAttack;
            _wallAttackTimer.OnTimerEnd += StopWallAttack;
        }

        private void OnDisable()
        {
            InputManager.Instance.PlayerActions.Attack.started -= Attack;
            InputManager.Instance.PlayerActions.WallBreaking.started -= WallAttack;
            
            _attackTimer.OnTimerEnd -= StopAttack;
            _wallAttackTimer.OnTimerEnd -= StopWallAttack;
        }

        #region ATTACK
        private AttackType GetAttackType()
        {
            Vector2 direction = _movementAction.ReadValue<Vector2>().normalized;
            float angle = Mathf.Abs(Mathf.Asin(direction.y) * Mathf.Rad2Deg);

            AttackType attackType = AttackType.Horizontal;

            if (angle >= _attackData.angleDirectionOffset)
            {
                if (direction.y > 0)
                    attackType = AttackType.Upwards;
                else if (!_playerMovement.IsGrounded)
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

        private void Attack(InputAction.CallbackContext context)
        {
            if (IsAttacking) return;
            
            _lastAttackInfo.Type = GetAttackType();
            _lastAttackInfo.Direction = GetAttackDirection(_lastAttackInfo.Type);
            
            if (context.ReadValueAsButton())
            {
                if (_lastAttackInfo.Type == AttackType.Downwards && _playerMovement.IsGrounded)
                    return;
                
                _isPlayerAttacking = true;
                _playerAnimations.SetAttackAnimation((int) _lastAttackInfo.Type);
            }
        }

        private void CheckAttackCollisions()
        {
            Vector3 position = _lastAttackInfo.Type switch
            {
                AttackType.Downwards => _bottomAttackPoint.position,
                AttackType.Upwards => _upperAttackPoint.position,
                AttackType.Horizontal => _horizontalAttackPoint.position,
                _ => Vector3.zero
            };
            
            Collider2D[] entities = Physics2D.OverlapCircleAll(position, _attackRadius, _hurtboxLayer);
            foreach (var entity in entities)
            {
                if (entity.transform.parent.TryGetComponent(out EntityHealth entityHealth) && !entity.CompareTag("Player"))
                {
                    if (!entityHealth.IsInvulnerable && entityHealth.damageable)
                    {
                        int damage = Mathf.CeilToInt(_attackData.attackDamage * DamageMultiplier);
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

        // called at the end of the attack animation
        private void StopAttack()
        {
            _isPlayerAttacking = false;
            _hasCollided = false;
            _attackTimer.Reset();
        }

        private void ApplyAttackForces(EntityHealth entityHealth)
        { 
            if (_lastAttackInfo.Type == AttackType.Downwards
                && entityHealth.giveUpwardForce
                && !_playerMovement.IsGrounded)
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
        
        #region WALL BREAKING
        private void WallAttack(InputAction.CallbackContext context)
        {
            if (!_abilitiesData.breakWalls) return;
            if (IsAttacking) return;
            
            if (context.ReadValueAsButton())
            {
                _isPlayerWallAttacking = true;
                _playerAnimations.SetWallAttackAnimation();
            }
        }

        private void CheckBreakableWalls()
        {
            Collider2D[] walls = Physics2D.OverlapCircleAll(_horizontalAttackPoint.position, _attackRadius);
            foreach (var wall in walls)
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
        
        // called at the end of the attack animation
        private void StopWallAttack()
        {
            _isPlayerWallAttacking = false;
            _hasCollided = false;
            _wallAttackTimer.Reset();
        }
        #endregion

        #region DEBUG
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_horizontalAttackPoint.position, _attackRadius);
            Gizmos.DrawWireSphere(_upperAttackPoint.position, _attackRadius);
            Gizmos.DrawWireSphere(_bottomAttackPoint.position, _attackRadius);
        }
        #endregion
    }
}