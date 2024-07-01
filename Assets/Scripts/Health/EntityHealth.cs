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
        [SerializeField] private HealthController _health;

        private ScreenShakeSource _screenShakeSource;
        
        private bool _hit;

        public bool IsInvulnerable => _hit;

        private void Awake()
        {
            _screenShakeSource = GetComponent<ScreenShakeSource>();
        }

        private void Start()
        {
            _health = new HealthController(maxHealth);
        }

        public void Damage(int amount, bool screenShake)
        {
            if (damageable && !_hit && _health.CurrentHealth > 0)
            {
                _hit = true;
                _health.Damage(amount);
                if (_health.CurrentHealth > 0)
                {
                    if (_screenShakeSource && screenShake)
                        _screenShakeSource.TriggerScreenShake();
                    
                    StartCoroutine(TurnOffHit());
                }
            }
        }

        private IEnumerator TurnOffHit()
        {
            yield return new WaitForSeconds(invulnerabilityTime);
            _hit = false;
        }
    }
}