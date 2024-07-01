using System;
using System.Collections;
using CameraSystem;
using GlobalVariables;
using PlayerController;
using UnityEngine;

namespace Health
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private IntReference _maxHealth;
        [SerializeField] private IntReference _currentHealth;
        [SerializeField] private float _invulnerabilityTime;

        private PlayerMovement _playerMovement;
        private ScreenShakeSource _screenShakeSource;
        
        private bool _hit;

        public event Action OnShieldLost;
        private bool _hasShield;
        public bool HasShield
        {
            get => _hasShield;
            set
            {
                _hasShield = value;
                if (!_hasShield)
                    OnShieldLost?.Invoke();
            }
        }

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _screenShakeSource = GetComponent<ScreenShakeSource>();
        }

        private void Start()
        {
            _currentHealth.Value = _maxHealth.Value;
        }

        public void Damage(int amount, int attackDirection)
        {
            if (!_hit && _currentHealth.Value > 0)
            {
                _hit = true;
                if (HasShield)
                {
                    HasShield = false;
                }
                else
                {
                    UpdateHealth(-amount);
                }
                
                _playerMovement.ApplyDamageKnockBack(attackDirection);
                if (_screenShakeSource)
                    _screenShakeSource.TriggerScreenShake();
                
                StartCoroutine(TurnOffHit());
            }
        }

        private void UpdateHealth(int amount)
        {
            _currentHealth.Value = Mathf.Clamp(_currentHealth.Value + amount, 0, _maxHealth);
            if (_currentHealth.Value <= 0)
            {
                Debug.Log("Has muerto");
            }
        }
        
        private IEnumerator TurnOffHit()
        {
            yield return new WaitForSeconds(_invulnerabilityTime);
            _hit = false;
        }
    }
}