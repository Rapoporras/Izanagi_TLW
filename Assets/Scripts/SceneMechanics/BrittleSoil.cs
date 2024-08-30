using UnityEngine;
using Utils;

namespace SceneMechanics
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BrittleSoil : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _timeToBreak;

        [Header("Debug")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _breakingDebugColor;
        
        private bool _hasPlayerTouchedGround;
        private Timer _timer;

        private void Awake()
        {
            _timer = new Timer(_timeToBreak);
            _timer.OnTimerEnd += DestroySoil;
        }

        private void Update()
        {
            if (_hasPlayerTouchedGround)
            {
                _timer.Tick(Time.deltaTime);
            }
        }

        private void OnDisable()
        {
            _timer.OnTimerEnd -= DestroySoil;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_hasPlayerTouchedGround) return;

            if (other.transform.CompareTag("Player"))
            {
                _hasPlayerTouchedGround = true;
                _spriteRenderer.color = _breakingDebugColor;
            }
        }
        
        private void DestroySoil()
        {
            // add some effects here
            Destroy(gameObject);
        }
    }
}