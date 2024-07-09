using System.Collections;
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
        
        private bool _hit;

        private void Start()
        {
            _health = new HealthController(maxHealth);
        }

        public void Damage(int amount)
        {
            if (damageable && !_hit && _health.CurrentHealth > 0) // se podria quitar la ultima condicion
            {
                _hit = true;
                _health.Damage(amount);
                if (_health.CurrentHealth > 0)
                {
                    StartCoroutine(TurnOffHit());
                }
            }
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
    }
}