using System.Collections;
using CameraSystem;
using Cinemachine;
using GameEvents;
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
        
        [Header("Screen Shake")]
        public ScreenShakeProfile screenShakeProfile;
        public ScreenShakeDataEvent screenShakeEvent;
        
        private CinemachineImpulseSource _impulseSource;
        private ScreenShakeData _screenShakeData;
        
        private bool _hit;

        public bool IsInvulnerable => _hit;

        private void Start()
        {
            _health = new HealthController(maxHealth);

            _impulseSource = GetComponent<CinemachineImpulseSource>();
            _screenShakeData.profile = screenShakeProfile;
            _screenShakeData.impulseSource = _impulseSource;
        }

        public void Damage(int amount)
        {
            if (damageable && !_hit && _health.CurrentHealth > 0)
            {
                _hit = true;
                _health.Damage(amount);
                if (_health.CurrentHealth > 0)
                {
                    if (screenShakeEvent)
                        screenShakeEvent.Raise(_screenShakeData);
                    
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