using Enemies.Kappa;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace Enemies.Kappas
{
    public class MiniKappasSpawner : MonoBehaviour
    {
        [Header("Spawner settings")]
        [SerializeField] private int _kappasToSpawn = 3;
        [SerializeField] private float _spawnDuration = 1f;
        [SerializeField] private GameObject _miniKappaPrefab;

        [Header("Player Detection Settings")]
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private LayerMask _groundLayer;
        [Space(5)]
        [SerializeField, Min(0.1f)] private float _verticalOffset = 0.1f;
        [SerializeField, Min(0.5f)] private float _horizontalDistance = 1f;

        [Header("Debugging")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _spawningDebugColor;

        // spawn
        private int _miniKappasSpawned;
        private Timer _spawnTimer;

        // player detection
        private const int RayCount = 3;
        private float _raySpacing;

        private bool _isActive;

        private void Awake()
        {
            float durationBetweenSpawn = _spawnDuration / _kappasToSpawn;
            _spawnTimer = new Timer(durationBetweenSpawn);

            _isActive = false;
        }

        private void Update()
        {
            if (_isActive)
            {
                _spawnTimer.Tick(Time.deltaTime);
            }
            else
            {
                CheckRaycasts();
            }
        }

        private void OnEnable() => _spawnTimer.OnTimerEnd += SpawnMiniKappa;
        
        private void OnDisable() => _spawnTimer.OnTimerEnd -= SpawnMiniKappa;

        private void SpawnMiniKappa()
        {
            // instantiate here the mini kappa
            GameObject miniKappaInstance = Instantiate(_miniKappaPrefab, transform.position, quaternion.identity);
            miniKappaInstance.GetComponent<MiniKappaAI>().SetUpBehaviourTree();
            _miniKappasSpawned++;
            Debug.Log($"Instantiating mini kappa -- {_miniKappasSpawned}");
            
            if (_miniKappasSpawned >= _kappasToSpawn)
            {
                _isActive = false;
                // add some effects or animation here
                Destroy(gameObject);
            }
            else
            {
                _spawnTimer.Reset();
            }
        }
        
        private void CheckRaycasts()
        {
            _raySpacing = _horizontalDistance / (RayCount - 1);
            
            Vector3 startRayOrigin = transform.position;
            startRayOrigin.x -= _raySpacing;
            startRayOrigin.y -= _verticalOffset;

            for (int i = 0; i < RayCount; i++)
            {
                Vector2 rayOrigin = startRayOrigin;
                rayOrigin += Vector2.right * (_raySpacing * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity,
                    _playerLayer | _groundLayer);

                if (hit && hit.transform.CompareTag("Player"))
                {
                    _isActive = true;
                    _spriteRenderer.color = _spawningDebugColor; // just for debugging
                    return;
                }
                
#if UNITY_EDITOR
                Debug.DrawLine(rayOrigin, rayOrigin + (Vector2.down * hit.distance), Color.red);
#endif
            }
        }
    }
}

