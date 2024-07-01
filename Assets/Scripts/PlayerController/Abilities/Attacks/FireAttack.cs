using Health;
using UnityEngine;

namespace PlayerController.Abilities
{
    public class FireAttack : BaseAreaAttack
    {

        private float _timer;
        
        public float Duration { get; set; }
        public int Damage { get; set; }

        private void Start()
        {
            _timer = Duration;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer > 0)
            {
                Collider2D[] entities = Physics2D.OverlapAreaAll(_pointA.position, _pointB.position);
                foreach (var entity in entities)
                {
                    if (entity.TryGetComponent(out EntityHealth entityHealth) && !entity.CompareTag("Player"))
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