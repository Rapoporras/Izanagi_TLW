using System;
using System.Collections;
using CameraSystem;
using UnityEngine;

namespace Health
{
    public class EntityHealth : MonoBehaviour
    {
        [Header("Settings")]
        public bool giveUpwardForce;
        public bool damageable;
        public int maxHealth;
        public float invulnerabilityTime;

        [Space(10)]
        [SerializeField] private HealthController _health = new HealthController();
        
        private ScreenShakeSource _screenShakeSource;
        private FlashEffect _flashEffect;
        
        private bool _hit;

        public bool IsInvulnerable => _hit;
        public int CurrentHealth => _health.CurrentHealth;

        private void Awake()
        {
            _screenShakeSource = GetComponent<ScreenShakeSource>();
            _flashEffect = GetComponent<FlashEffect>();
            
            ResetHealth();
        }

        public void Damage(int amount, bool screenShake)
        {
            if (damageable && !_hit && _health.CurrentHealth > 0)
            {
                _hit = true;
                _health.Damage(amount);
                
                if (_health.CurrentHealth > 0)
                    StartCoroutine(TurnOffHit());
                
                if (_screenShakeSource && screenShake)
                    _screenShakeSource.TriggerScreenShake();

                if (_flashEffect)
                    _flashEffect.CallDamageFlash();
            }
        }

        public void ResetHealth()
        {
            _hit = false;
            _health.ResetHealth(maxHealth);
        }

        public float GetHealthPercent()
        {
            return _health.CurrentHealth * 1f / maxHealth;
        }

        private IEnumerator TurnOffHit()
        {
            yield return new WaitForSeconds(invulnerabilityTime);
            _hit = false;
        }

        public void AddListenerDeathEvent(Action listener)
        {
            _health.OnDeathEvent += listener;
        }
        
        public void RemoveListenerDeathEvent(Action listener)
        {
            _health.OnDeathEvent -= listener;
        }

        public void AddListenerOnHit(Action listener)
        {
            _health.OnHealthUpdated += listener;
        }
        
        public void RemoveListenerOnHit(Action listener)
        {
            _health.OnHealthUpdated -= listener;
        }
    }
}