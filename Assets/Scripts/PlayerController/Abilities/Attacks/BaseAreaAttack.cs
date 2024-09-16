using System.Collections.Generic;
using UnityEngine;

namespace PlayerController.Abilities
{
    public class BaseAreaAttack : MonoBehaviour
    {
        [SerializeField] protected LayerMask _hurtboxLayer;
        
        public float Duration { get; set; }
        public int Damage { get; set; }

        protected Collider2D _hitbox;
        protected ContactFilter2D _contactFilter;

        protected List<Collider2D> _overlappedColliders = new List<Collider2D>();

        protected virtual void Awake()
        {
            _hitbox = GetComponent<Collider2D>();

            _contactFilter = new ContactFilter2D();
            _contactFilter.SetLayerMask(_hurtboxLayer);
            _contactFilter.useTriggers = true;
        }
    }
}