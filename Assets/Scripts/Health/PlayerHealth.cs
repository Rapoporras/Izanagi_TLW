using System;
using System.Collections;
using CameraSystem;
using Cinemachine;
using GameEvents;
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

        [Header("Screen Shake")]
        [SerializeField] private ScreenShakeProfile _screenShakeProfile;
        [SerializeField] private ScreenShakeDataEvent _screenShakeEvent;

        private PlayerMovement _playerMovement;
        
        private CinemachineImpulseSource _impulseSource;
        private ScreenShakeData _screenShakeData;
        
        private bool _hit;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void Start()
        {
            _currentHealth.Value = _maxHealth.Value;

            _screenShakeData.profile = _screenShakeProfile;
            _screenShakeData.impulseSource = _impulseSource;
        }

        public void Damage(int amount, int attackDirection)
        {
            if (!_hit && _currentHealth.Value > 0)
            {
                _hit = true;
                UpdateHealth(-amount);
                _playerMovement.ApplyDamageKnockBack(attackDirection);
                if (_currentHealth > 0)
                {
                    if (_screenShakeEvent)
                        _screenShakeEvent.Raise(_screenShakeData);
                    
                    StartCoroutine(TurnOffHit());
                }
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