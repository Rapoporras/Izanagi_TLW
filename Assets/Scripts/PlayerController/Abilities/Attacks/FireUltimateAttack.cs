using Health;
using UnityEngine;

namespace PlayerController.Abilities
{
    public class FireUltimateAttack : MonoBehaviour
    {
        [SerializeField] private LayerMask _hurtboxLayer;
        
        public int damage;
        public float duration;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            InputManager.Instance.DisablePlayerActions();
            Time.timeScale = 0f;
        }

        public void AttackEnemiesInScreen() // called in an animation keyframe
        {
            float height = _mainCamera.orthographicSize * 2f;
            float width = height * _mainCamera.aspect;

            Vector3 center = _mainCamera.transform.position;
            Vector2 pointA = new Vector2(
                center.x - (width / 2f),
                center.y - (height / 2f));
            Vector2 pointB = new Vector2(
                center.x + (width / 2f),
                center.y + (height / 2f));
            
            Collider2D[] entities = Physics2D.OverlapAreaAll(pointA, pointB, _hurtboxLayer);
            foreach (var entity in entities)
            {
                if (entity.CompareTag("Enemy") && entity.transform.parent.TryGetComponent(out EntityHealth entityHealth))
                {
                    entityHealth.Damage(damage, false);
                }
            }
            
            Time.timeScale = 1f;
            Destroy(gameObject);
            InputManager.Instance.EnablePlayerActions();
        }
    }
}