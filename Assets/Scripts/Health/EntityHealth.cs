using System.Collections;
using UnityEngine;

namespace Health
{
    public class EntityHealth : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _damageable;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _invulnerabiltyTime;
        [field: SerializeField] public bool GiveUpwardForce { get; private set; }

        private bool _hit;

        [Space(10)]
        [SerializeField] private HealthController _health;

        private void Start()
        {
            _health = new HealthController(_maxHealth);
        }

        public void Damage(int amount)
        {
            if (_damageable && !_hit && _health.CurrentHealth > 0) // se podria quitar la ultima condicion
            {
                _hit = true;
                _health.Damage(amount);
                if (_health.CurrentHealth > 0)
                {
                    StartCoroutine(TurnOffHit());
                }
            }
        }

        private IEnumerator TurnOffHit()
        {
            yield return new WaitForSeconds(_invulnerabiltyTime);
            _hit = false;
        }
    }
}