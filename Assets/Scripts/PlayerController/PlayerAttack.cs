using PlayerController.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float _angleOffset;
        
        private PlayerAnimations _playerAnimations;
        private PlayerMovement _playerMovement;
        
        private PlayerInputActions _playerInputActions;
        private InputAction _movementAction;

        [SerializeField, ReadOnly] private bool _isPlayerAttacking;

        private void Awake()
        {
            _playerAnimations = GetComponent<PlayerAnimations>();
            _playerMovement = GetComponent<PlayerMovement>();
            
            _playerInputActions = new PlayerInputActions();
            _isPlayerAttacking = false;
        }

        private void OnEnable()
        {
            _playerInputActions.Player.Attack.started += Attack;
            _playerInputActions.Player.Attack.Enable();

            _movementAction = _playerInputActions.Player.Movement;
            _movementAction.Enable();
        }

        private int GetAttackType()
        {
            Vector2 direction = _movementAction.ReadValue<Vector2>().normalized;
            float angle = Mathf.Abs(Mathf.Asin(direction.y) * Mathf.Rad2Deg);

            int attackType = 0; // horizontal attack

            if (angle >= 30f)
            {
                // 1 -> downwards
                // 2 -> upwards
                attackType = direction.y < 0 ? 1 : 2;
            }
            
            return attackType;
        }

        private void Attack(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton() && !_isPlayerAttacking
                && _playerMovement.CurrentState != PlayerStates.Dashing)
            {
                _isPlayerAttacking = true;
                int attackType = GetAttackType();
                _playerAnimations.SetAttackAnimation(attackType);
            }
        }

        private void StopAttack()
        {
            _isPlayerAttacking = false;
        }
        
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"<color=black><size=50>Input: {_movementAction.ReadValue<Vector2>().normalized}</size></color>");
            GUILayout.EndHorizontal();
        }
    }
}