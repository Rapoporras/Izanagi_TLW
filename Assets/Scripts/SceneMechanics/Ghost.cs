using CustomAttributes;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace SceneMechanics
{
    public class Ghost : MonoBehaviour
    {
        enum GhostState
        {
            Active,
            Inactive
        }

        struct Bounds
        {
            public float minX;
            public float maxX;
            public float minY;
            public float maxY;
        }

        [Header("Alpha")] [SerializeField, Range(0, 1)]
        private float _maxAlpha = 1f;

        [SerializeField, Range(0, 0.5f)] private float _alphaDistance = 0.2f;

        [Header("Movement")]
        [SerializeField, MinMax(0, 10)] private Vector2 _speedRange;
        [SerializeField, MinMax(0, 10)] private Vector2 _frequencyRange;
        [SerializeField, MinMax(0, 10)] private Vector2 _amplitudeRange;
        [Space(10)]
        [SerializeField, MinMax(0,5)] private Vector2 _inactiveTimeRange;

        [Header("Bounds")] [SerializeField] private Transform _topLeftPosition;
        [SerializeField] private Transform _bottomRightPosition;

        [SerializeField, ReadOnly] private GhostState _state;

        private Bounds _bounds;
        private Vector2 _movDir;
        private SpriteRenderer _spriteRenderer;

        private float _speed;
        private float _frequency;
        private float _amplitude;

        private Vector3 _initialPos;
        private float _finalX;
        private float _pathProgress;

        private Timer _inactivityTimer;

        private bool _isPlayerInTown;

        public bool IsPlayerInTown
        {
            get => _isPlayerInTown;
            set
            {
                _isPlayerInTown = value;
                if (value && _state == GhostState.Inactive)
                {
                    Spawn();
                }
            }
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _bounds = new Bounds
            {
                minX = _topLeftPosition.position.x,
                maxX = _bottomRightPosition.position.x,
                minY = _bottomRightPosition.position.y,
                maxY = _topLeftPosition.position.y
            };
            _inactivityTimer = new Timer(Random.Range(_inactiveTimeRange.x, _inactiveTimeRange.y));

            SetAlpha(0);
            _state = GhostState.Inactive;
        }

        private void Update()
        {
            switch (_state)
            {
                case GhostState.Active:
                    ApplyMovement();
                    ChangeAlpha();
                    break;
                case GhostState.Inactive:
                    if (!_isPlayerInTown) break;
                    
                    _inactivityTimer.Tick(Time.deltaTime);
                    if (_inactivityTimer.Finished)
                        Spawn();
                    break;
            }
        }

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            SetMovParameters();
            
            float dir = Random.Range(0, 2) == 0 ? -1f : 1f;
            _movDir = new Vector2(dir, 0);

            _initialPos = new Vector3(
                dir < 0 ? _bounds.maxX : _bounds.minX,
                Random.Range(_bounds.minY + _amplitude, _bounds.maxY - _amplitude),
                0
            );

            transform.position = _initialPos;
            _finalX = dir < 0 ? _bounds.minX : _bounds.maxX;
            SetAlpha(0);

            _pathProgress = 0f;
            _inactivityTimer.Reset(Random.Range(_inactiveTimeRange.x, _inactiveTimeRange.y));
            
            _state = GhostState.Active;
        }

        private void SetAlpha(float alpha)
        {
            if (_spriteRenderer)
            {
                Color color = _spriteRenderer.color;
                color.a = alpha;
                _spriteRenderer.color = color;
            }
        }

        private void ChangeAlpha()
        {
            if (_pathProgress <= _alphaDistance)
            {
                float lerp = Mathf.InverseLerp(0, _alphaDistance, _pathProgress);
                float alpha = Mathf.Lerp(0, _maxAlpha, lerp);
                SetAlpha(alpha);
            }
            else if (_pathProgress >= 1 - _alphaDistance)
            {
                float lerp = Mathf.InverseLerp(1 - _alphaDistance, 1, _pathProgress);
                float alpha = Mathf.Lerp(_maxAlpha, 0, lerp);
                SetAlpha(alpha);

                if (alpha == 0)
                {
                    _state = GhostState.Inactive;
                }
            }
        }

        private void ApplyMovement()
        {
            Vector3 deltaPos = new Vector3(
                _movDir.x * _speed * Time.deltaTime,
                Mathf.Sin(Time.time * _frequency) * _amplitude,
                0
            );

            Vector3 newPos = transform.position;
            newPos.x += deltaPos.x;
            newPos.y = _initialPos.y + deltaPos.y;
            transform.position = newPos;

            _pathProgress = Mathf.InverseLerp(_initialPos.x, _finalX, newPos.x);
        }

        private void SetMovParameters()
        {
            _speed = Random.Range(_speedRange.x, _speedRange.y);
            _frequency = Random.Range(_frequencyRange.x, _frequencyRange.y);
            _amplitude = Random.Range(_amplitudeRange.x, _amplitudeRange.y);
        }
    }
}
