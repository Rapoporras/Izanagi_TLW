using UnityEngine;

namespace PlayerController.Abilities
{
    public class BaseAreaAttack : MonoBehaviour
    {
        [SerializeField] protected LayerMask _hurtboxLayer;
        
        public float Duration { get; set; }
        public int Damage { get; set; }

        protected BoxCollider2D _hitbox;

        protected virtual void Awake()
        {
            _hitbox = GetComponent<BoxCollider2D>();
        }
    }
}