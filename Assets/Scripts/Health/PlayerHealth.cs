using System.Collections;
using GlobalVariables;
using PlayerController;
using UnityEngine;

namespace Health
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private IntReference _maxHealth;
        [SerializeField] private IntReference _currentHealth;
        [SerializeField] private float _invulnerabilityTime;

        private PlayerMovement _playerMovement;
        
        private bool _hit;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
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
                UpdateHealth(-amount);
                _playerMovement.ApplyDamageKnockBack(attackDirection);
                if (_currentHealth > 0)
                {
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