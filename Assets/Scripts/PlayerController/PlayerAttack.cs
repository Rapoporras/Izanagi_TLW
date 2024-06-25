using GlobalVariables;
using Health;
using PlayerController.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    public class PlayerAttack : MonoBehaviour
    {
        private enum AttackType
        {
            Horizontal, Downwards, Upwards
        }

        [Header("Data")]
        [SerializeField] private PlayerAttackData _attackData;
        [SerializeField] private PlayerAbilitiesData _abilitiesData;

        [Header("Manna")]
        [SerializeField] private IntReference _currentManna;
        [SerializeField] private IntReference _maxManna;
        [SerializeField] private IntReference _mannaContainers;
        [SerializeField] private int _mannaPerAttack;
        
        [Space(10), Header("Settings")]
        [SerializeField] private Transform _horizontalAttackPoint;
        [SerializeField] private Transform _upperAttackPoint;
        [SerializeField] private Transform _bottomAttackPoint;
        [SerializeField] private float _attackRadius;
        
        private PlayerAnimations _playerAnimations;
        private PlayerMovement _playerMovement;
        
        private InputAction _movementAction;

        private bool _isPlayerAttacking;
        private bool _hasCollided;
        private AttackInfo _lastAttackInfo;

        private int _mannaContainersFilled;

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
        }

        private void Update()
        {
            if (_isPlayerAttacking)
            {
                CheckAttackCollisions();
            }
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
            if (_isPlayerAttacking) return;
            // if (_playerMovement.CurrentState == PlayerStates.Dashing) return;
            // if (_playerMovement.CurrentState == PlayerStates.Damaged) return;
            
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
            
            Collider2D[] entities = Physics2D.OverlapCircleAll(position, _attackRadius);
            foreach (var entity in entities)
            {
                if (entity.TryGetComponent(out EntityHealth entityHealth) && !entity.CompareTag("Player"))
                {
                    if (!entityHealth.IsInvulnerable && entityHealth.damageable)
                    {
                        entityHealth.Damage(_attackData.attackDamage);
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
            _mannaContainersFilled = _currentManna * _mannaContainers / _maxManna;
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