using Health;
using UnityEngine;

namespace PlayerController.Abilities
{
    public class FireAttack : BaseAreaAttack
    {
        private float _timer;

        private void Start()
        {
            _timer = Duration;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer > 0)
            {
                Collider2D[] entities = Physics2D.OverlapAreaAll(_hitbox.bounds.min, _hitbox.bounds.max, _hurtboxLayer);
                foreach (var entity in entities)
                {
                    if (entity.transform.parent.TryGetComponent(out EntityHealth entityHealth) && !entity.CompareTag("Player"))
                    {
                        if (!entityHealth.IsInvulnerable && entityHealth.damageable)
                        {
                            entityHealth.Damage(Damage, false);
                        }
                    }
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}