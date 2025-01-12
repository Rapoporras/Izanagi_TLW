using Health;
using UnityEngine;
using Utils;

namespace PlayerController.Abilities
{
    public class FireAbilityAttack : BaseAreaAttack
    {
        [SerializeField] private ParticleSystem _particleSystem;
        private Timer _timer;

        private void Start()
        {
            _timer = new Timer(_particleSystem.main.duration);
            
            // _particleSystem.is
        }

        private void Update()
        {
            if (_particleSystem.isPlaying)
            {
                _timer.Tick(Time.deltaTime);
                _overlappedColliders.Clear();
                Physics2D.OverlapCollider(_hitbox, _contactFilter, _overlappedColliders);
                foreach (var entity in _overlappedColliders)
                {
                    if (entity.TryGetComponent(out IDamageable damageableArea) && entity.CompareTag("Enemy"))
                    {
                        damageableArea.Damage(Damage, false);
                    }
                }
            }
            else
            {
                Destroy(gameObject);
            }
            
            // _timer -= Time.deltaTime;
            // if (_timer > 0)
            // {
            //     _overlappedColliders.Clear();
            //     Physics2D.OverlapCollider(_hitbox, _contactFilter, _overlappedColliders);
            //     foreach (var entity in _overlappedColliders)
            //     {
            //         if (entity.TryGetComponent(out IDamageable damageableArea) && entity.CompareTag("Enemy"))
            //         {
            //             damageableArea.Damage(Damage, false);
            //         }
            //     }
            // }
            // else
            // {
            //     Destroy(gameObject);
            // }
        }
    }
}