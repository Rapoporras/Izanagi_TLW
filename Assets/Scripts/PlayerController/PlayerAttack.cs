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

        [SerializeField] private PlayerAttackData _attackData;
        
        [Space(10)]
        [SerializeField] private Transform _horizontalAttackPoint;
        [SerializeField] private Transform _upperAttackPoint;
        [SerializeField] private Transform _bottomAttackPoint;
        [SerializeField] private float _attackRadius;
        
        private Rigidbody2D _rb2d;
        private PlayerAnimations _playerAnimations;
        private PlayerMovement _playerMovement;
        
        private PlayerInputActions _playerInputActions;
        private InputAction _movementAction;

        private bool _isPlayerAttacking;
        private bool _hasCollided;
        private AttackInfo _lastAttackInfo;

        struct AttackInfo
        {
            public AttackType Type;
            public Vector2 Direction;
        }

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _playerAnimations = GetComponent<PlayerAnimations>();
            _playerMovement = GetComponent<PlayerMovement>();
            
            _playerInputActions = new PlayerInputActions();
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
            _playerInputActions.Player.Attack.started += Attack;
            _playerInputActions.Player.Attack.Enable();

            _movementAction = _playerInputActions.Player.Movement;
            _movementAction.Enable();
        }

        private AttackType GetAttackType()
        {
            Vector2 direction = _movementAction.ReadValue<Vector2>().normalized;
            float angle = Mathf.Abs(Mathf.Asin(direction.y) * Mathf.Rad2Deg);

            AttackType attackType = AttackType.Horizontal;

            if (angle >= _attackData.angleDirectionOffset)
            {
                attackType = direction.y < 0 ? AttackType.Downwards : AttackType.Upwards;
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
            if (_playerMovement.CurrentState == PlayerStates.Dashing) return;
            
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
                    entityHealth.Damage(_attackData.attackDamage);
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
                && entityHealth.GiveUpwardForce
                && !_playerMovement.IsGrounded)
            {
                _playerMovement.ApplyUpwardsKnockBack(_attackData.upwardsForce);
            }
            else if (_lastAttackInfo.Type == AttackType.Horizontal)
            {
                _playerMovement.ApplyHorizontalKnockBack(
                    _attackData.horizontalKnockBackSpeed,
                    _lastAttackInfo.Direction * -1f,
                    _attackData.horizontalKnockBackTime);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_horizontalAttackPoint.position, _attackRadius);
            Gizmos.DrawWireSphere(_upperAttackPoint.position, _attackRadius);
            Gizmos.DrawWireSphere(_bottomAttackPoint.position, _attackRadius);
        }
    }
}