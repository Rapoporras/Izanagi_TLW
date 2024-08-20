using Health;
using UnityEngine;

namespace PlayerController.Abilities
{
    public class FireAbilityAttack : BaseAreaAttack
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
                _overlappedColliders.Clear();
                Physics2D.OverlapCollider(_hitbox, _contactFilter, _overlappedColliders);
                foreach (var entity in _overlappedColliders)
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