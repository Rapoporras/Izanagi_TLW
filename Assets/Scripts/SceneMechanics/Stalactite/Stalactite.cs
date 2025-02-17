﻿using System.Collections.Generic;
using Health;
using PlayerController;
using StateMachine;
using UnityEngine;
using Utils;

namespace SceneMechanics.Stalactite
{
    public class Stalactite : BaseStateMachine<StalactiteStates>, IResettable
    {
        [Header("Settings")]
        [SerializeField] private bool _activateWithRaycasts = true;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private LayerMask _groundLayer;
        [Space(5)]
        [SerializeField, Min(0.1f)] private float _verticalOffset = 0.1f;
        [SerializeField, Min(0.5f)] private float _horizontalDistance = 1f;

        [Header("States Settings")]
        public float detachingDuration = 0.5f;
        [SerializeField] private float _fallingAcceleration = 40f;
        public float maxFallingVelocity = 30f;

        [Header("States (Debugging)")]
        [SerializeField] private bool _useDebugColors;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Space(5)]
        [SerializeField] private Color _idleDebugColor;
        [SerializeField] private Color _detachingDebugColor;
        [SerializeField] private Color _fallingDebugColor;
        [SerializeField] private Color _groundedDebugColor;

        private const int RayCount = 3;
        private float _raySpacing;

        [HideInInspector] public bool playerDetected;
        [HideInInspector] public float fallGravityScale;

        private Rigidbody2D _rb2d;
        private RaycastInfo _raycastInfo;
        private EntityHealth _entityHealth;
        private ContactDamage _contactDamage;
        
        [Header("Audio Settings")]
        private AudioSource _audioSource;
        [SerializeField] private List<AudioClip> _fallingAudio;
        [SerializeField] private AudioClip _hitFloorAudio;
        [SerializeField] private AudioClip _breakAudio;

        [Header("Dependencies")]
        [SerializeField] private ParticleSystem _particleSystem;

        private float _fallVel;
        private Vector3 _initialPos;

        public bool DamageActive
        {
            get => _contactDamage.isActive;
            set => _contactDamage.isActive = value;
        }
        
        public Vector2 Velocity
        {
            get => _rb2d.velocity;
            set => _rb2d.velocity = value;
        }

        public bool IsGrounded => _raycastInfo.HitInfo.Below;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _raycastInfo = GetComponent<RaycastInfo>();
            _entityHealth = GetComponent<EntityHealth>();
            _contactDamage = GetComponent<ContactDamage>();
            _audioSource = GetComponent<AudioSource>();

            fallGravityScale = Mathf.Abs(_fallingAcceleration / Physics2D.gravity.y);

            _initialPos = transform.position;
        }

        private void OnEnable()
        {
            _entityHealth.AddListenerDeathEvent(DestroyObject);
        }

        private void OnDisable()
        {
            _entityHealth.RemoveListenerDeathEvent(DestroyObject);
        }

        protected override void SetStates()
        {
            States.Add(StalactiteStates.Idle, new StalactiteIdleState(StalactiteStates.Idle, this));
            States.Add(StalactiteStates.Detaching, new StalactiteDetachingState(StalactiteStates.Detaching, this));
            States.Add(StalactiteStates.Falling, new StalactiteFallingState(StalactiteStates.Falling, this));
            States.Add(StalactiteStates.Grounded, new StalactiteGroundedState(StalactiteStates.Grounded, this));
            
            _currentState = States[StalactiteStates.Idle];
        }

        private void DestroyObject()
        {
            // add effects here
            _audioSource.clip = _breakAudio;
            _audioSource.Play();
            gameObject.SetActive(false);
        }
        
        public void CheckRaycasts()
        {
            if (!_activateWithRaycasts) return;
            
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
                    Activate();
                    _particleSystem.Stop();
                    return;
                }
                
#if UNITY_EDITOR
                Debug.DrawLine(rayOrigin, rayOrigin + (Vector2.down * hit.distance), Color.red);
#endif
            }
        }

        [ContextMenu("Activate")]
        public void Activate()
        {
            playerDetected = true;
        }

        public void SetGravityScale(float scale)
        {
            _rb2d.gravityScale = scale;
        }

        /// <summary>
        /// Change sprite color, just for debugging
        /// </summary>
        public void SetColor()
        {
            if (!_useDebugColors) return;
            
            switch (_currentState.StateKey)
            {
                case StalactiteStates.Idle:
                    _spriteRenderer.color = _idleDebugColor;
                    break;
                case StalactiteStates.Detaching:
                    _spriteRenderer.color = _detachingDebugColor;
                    break;
                case StalactiteStates.Falling:
                    _spriteRenderer.color = _fallingDebugColor;
                    break;
                case StalactiteStates.Grounded:
                    _spriteRenderer.color = _groundedDebugColor;
                    break;
            }
        }
        
        #region AUDIO
        public void PlayFloorHitAudio()
        {
            _audioSource.clip = _hitFloorAudio;
            _audioSource.Play();
        }

        public void PlayFallAudio()
        {
            _audioSource.clip = _fallingAudio[Random.Range(0,3)];
            _audioSource.Play();
        }
        #endregion

        public void ResetObject()
        {
            _currentState.ExitState();
            _currentState = States[StalactiteStates.Idle];
            _currentState.EnterState();
            
            transform.position = _initialPos;
            gameObject.SetActive(true);
        }
    }
}