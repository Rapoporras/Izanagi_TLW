﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enemies.Kappa;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class MiniKappasSpawner : MonoBehaviour, IResettable
    {
        [Header("Spawner settings")]
        [SerializeField] private int _kappasToSpawn = 3;
        [SerializeField] private float _spawnDuration = 1f;
        [SerializeField] private MiniKappaAI _miniKappaPrefab;
        [SerializeField] private Transform _spawnPosition;

        [Header("Player Detection Settings")]
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private LayerMask _groundLayer;
        [Space(5)]
        [SerializeField, Min(0.1f)] private float _verticalOffset = 0.1f;
        [SerializeField, Min(0.5f)] private float _horizontalDistance = 1f;

        [Header("Dependencies")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Sprite _brokenSprinte;
        
        [Header("Debugging")]
        [SerializeField] private Color _spawningDebugColor;

        private AudioSource _audioSource;

        // player detection
        private const int RayCount = 3;
        private float _raySpacing;

        private bool _playerDetected;

        private List<MiniKappaAI> _miniKappasSpawned = new List<MiniKappaAI>();
        private Sprite _initialSprite;

        private void Awake()
        {
            _particleSystem.Stop();
            _audioSource = GetComponent<AudioSource>();

            _initialSprite = _spriteRenderer.sprite;
        }

        private void Update()
        {
            if (!_playerDetected)
            {
                CheckRaycasts();
            }
        }

        private IEnumerator SpawnMiniKappa()
        {
            for (int i = 0; i < 3; i++)
            {
                PlaySpawnTween();
                   
                yield return new WaitForSeconds(_spawnDuration / _kappasToSpawn);
            }

            _spriteRenderer.sprite = _brokenSprinte;
            
            _miniKappasSpawned.Clear();
            if (_spriteRenderer.sprite == _brokenSprinte)
            {
                _particleSystem.Play();
                for(int i=0; i < _kappasToSpawn; i++)
                {
                    MiniKappaAI miniKappaInstance = Instantiate(_miniKappaPrefab, _spawnPosition.transform.position, Quaternion.identity);
                    miniKappaInstance.SetUpBehaviourTree();
                    
                    _miniKappasSpawned.Add(miniKappaInstance);
                }
            }
        }

        private void PlaySpawnTween()
        {
            transform.DOPunchScale(Vector3.down * 0.2f, 0.1f);
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
                    _playerDetected = true;
                    _audioSource.Play();
                    StartCoroutine(SpawnMiniKappa());
                    // _spriteRenderer.color = _spawningDebugColor; // just for debugging
                    return;
                }
                
#if UNITY_EDITOR
                Debug.DrawLine(rayOrigin, rayOrigin + (Vector2.down * hit.distance), Color.red);
#endif
            }
        }

        public void ResetObject()
        {
            _spriteRenderer.sprite = _initialSprite;
            _playerDetected = false;
            
            foreach (var miniKappa in _miniKappasSpawned)
            {
                miniKappa.ResetObject();
            }
            
            _miniKappasSpawned.Clear();
        }
    }
}

