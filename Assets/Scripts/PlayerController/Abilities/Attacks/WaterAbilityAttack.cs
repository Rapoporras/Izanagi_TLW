using Health;
using UnityEngine;

namespace PlayerController.Abilities
{
    public class WaterAbilityAttack : BaseAreaAttack
    {
        [SerializeField] private LayerMask _collisionLayers;
        [SerializeField] private float _skinWidth = 0.015f;
        [SerializeField] private float _rayVerticalLenght = 0.05f;
        [SerializeField] private float _rayVerticalOffset = 0.1f;
        
        public float Speed { get; set; }

        private float _timer;

        private Rigidbody2D _rb2d;
        private RaycastInfo _raycastInfo;

        protected override void Awake()
        {
            base.Awake();
            
            _rb2d = GetComponent<Rigidbody2D>();
            _raycastInfo = GetComponent<RaycastInfo>();
        }
        
        private void Start()
        {
            _timer = Duration;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer > 0 && !DestroyWave())
            {
                ApplyMovement();
                ApplyDamage();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void ApplyMovement()
        {
            Vector2 deltaPosition = new Vector2(transform.right.x * (Speed * Time.deltaTime), 0f);
            _rb2d.MovePosition(_rb2d.position + deltaPosition);
        }

        private void ApplyDamage()
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

        private bool DestroyWave()
        {
            // GroundCheck();
            // return false;
            return GroundCheck() || _raycastInfo.HitInfo.Left || _raycastInfo.HitInfo.Right;
        }

        private bool GroundCheck()
        {
            Bounds bounds = _hitbox.bounds;
            bounds.Expand(_skinWidth * -2);

            Vector2 rightRayOrigin = new Vector2(bounds.max.x + _rayVerticalOffset, bounds.min.y);
            RaycastHit2D rightHit = Physics2D.Raycast(
                rightRayOrigin,
                Vector2.down,
                _rayVerticalLenght,
                _collisionLayers);
            
            Vector2 leftRayOrigin = new Vector2(bounds.min.x - _rayVerticalOffset, bounds.min.y);
            RaycastHit2D leftHit = Physics2D.Raycast(
                leftRayOrigin,
                Vector2.down,
                _rayVerticalLenght,
                _collisionLayers);

            Color raycastColor = rightHit ? Color.green : Color.red;
            Debug.DrawRay(rightRayOrigin, Vector2.down * _rayVerticalLenght, raycastColor);
            
            raycastColor = leftHit ? Color.green : Color.red;
            Debug.DrawRay(leftRayOrigin, Vector2.down * _rayVerticalLenght, raycastColor);
            
            return !rightHit || !leftHit;
        }
    }
}