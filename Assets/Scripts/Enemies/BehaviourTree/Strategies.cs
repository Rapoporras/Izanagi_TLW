using System;
using System.Collections;
using System.Collections.Generic;
using Enemies.Kappa;
using Health;
using PlayerController;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Utils.CustomLogs;

namespace Enemies.BehaviourTree
{
    public interface IStrategy
    {
        Node.Status Process();

        void Reset()
        {
            //default
        }
    }

    public class ActionStrategy : IStrategy
    {
        private readonly Action _actionToDo;

        public ActionStrategy(Action actionToDo)
        {
            _actionToDo = actionToDo;
        }

        public Node.Status Process()
        {
            _actionToDo();
            return Node.Status.Success;
        }
    }

    public class ConditionStrategy : IStrategy
    {
        private readonly Func<bool> _predicate;

        public ConditionStrategy(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        public Node.Status Process()
        {
            return _predicate() ? Node.Status.Success : Node.Status.Failure;
        }
    }

    public class ChaseStrategyWithJump : IStrategy
    {
        private GameObject _enemy;
        private Transform _enemyTransform;
        private Transform _playerTransform;
        private float _detectionRadius;
        private float _chaseSpeed;
        private float _timeToStopChase;
        private float _stopChaseTimer;

        private LayerMask _groundLayer;
        private float _jumpForce;
        private bool _isGrounded;
        private bool _shouldJump;

        private float _areaWidth;
        private float _areaHeight;



        public ChaseStrategyWithJump(GameObject enemy, GameObject player, float detectionRadius, float chaseSpeed,
            float timeToStopChase, LayerMask groundLayer, float jumpForce, float areaWidth, float areaHeight)
        {
            _enemy = enemy;
            _enemyTransform = enemy.transform;
            _playerTransform = player.transform;
            _detectionRadius = detectionRadius;
            _chaseSpeed = chaseSpeed;
            _timeToStopChase = timeToStopChase;
            _stopChaseTimer = _timeToStopChase;
            _groundLayer = groundLayer;
            _jumpForce = jumpForce;
            _areaWidth = areaWidth;
            _areaHeight = areaHeight;
        }

        public Node.Status Process()
        {

            Rigidbody2D rb = _enemy.GetComponent<Rigidbody2D>();

            _isGrounded = Physics2D.Raycast(_enemyTransform.position, Vector2.down, 0.1f, _groundLayer);

            float playerDirection = Mathf.Sign(_playerTransform.position.x - _enemyTransform.position.x);

            bool isPlayerAbove = Physics2D.OverlapBox(_enemyTransform.position + new Vector3(0f, 4f, 0f),
                new Vector2(_areaWidth, _areaHeight), 0, 1 << _playerTransform.gameObject.layer);


            if (_stopChaseTimer > 0)
            {
                if (Vector3.Distance(_enemyTransform.position, _playerTransform.position) > _detectionRadius)
                {
                    // if it's out of range start decreasing timer
                    _stopChaseTimer -= Time.deltaTime;
                }
                else
                {
                    // if it's in range reset timer
                    _stopChaseTimer = _timeToStopChase;
                }

                RaycastHit2D groundInFront = Physics2D.Raycast(_enemyTransform.position,
                    new Vector2(playerDirection, 0), 0.75f, _groundLayer);
                RaycastHit2D groundInFrontBelow = Physics2D.Raycast(_enemyTransform.position + new Vector3(playerDirection, 0, 0),
                    Vector2.down, 5f, _groundLayer);
                RaycastHit2D platformAbove = Physics2D.Raycast(_enemyTransform.position,
                    Vector2.up, 3f, _groundLayer);

                if (_isGrounded)
                    rb.velocity = new Vector2(playerDirection * _chaseSpeed, rb.velocity.y);

                if ((!groundInFrontBelow.collider || groundInFront || isPlayerAbove) && _isGrounded)
                {
                    rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                }
            }
            else
            {
                //chase ended, node returns
                Debug.Log("Chase ended");
                rb.velocity -= new Vector2(1f, 1f);
                return Node.Status.Success;
            }

            return Node.Status.Running;
        }

        public void Reset()
        {
            _stopChaseTimer = _timeToStopChase;
        }
    }

    public class ChaseStrategy : IStrategy
    {
        private GameObject _enemy;
        private Transform _enemyTransform;
        private Transform _playerTransform;
        private float _detectionRadius;
        private float _chaseSpeed;
        private float _timeToStopChase;
        private float _stopChaseTimer;

        private float _areaWidth;
        private float _areaHeight;

        public ChaseStrategy(GameObject enemy, GameObject player, float detectionRadius, float chaseSpeed,
            float timeToStopChase)
        {
            _enemy = enemy;
            _enemyTransform = enemy.transform;
            _playerTransform = player.transform;
            _detectionRadius = detectionRadius;
            _chaseSpeed = chaseSpeed;
            _timeToStopChase = timeToStopChase;
            _stopChaseTimer = _timeToStopChase;
        }

        public Node.Status Process()
        {

            Rigidbody2D rb = _enemy.GetComponent<Rigidbody2D>();

            float playerDirection = Mathf.Sign(_playerTransform.position.x - _enemyTransform.position.x);

            if (_stopChaseTimer > 0)
            {
                if (Vector3.Distance(_enemyTransform.position, _playerTransform.position) > _detectionRadius)
                {
                    // if it's out of range start decreasing timer
                    _stopChaseTimer -= Time.deltaTime;
                }
                else
                {
                    // if it's in range reset timer
                    _stopChaseTimer = _timeToStopChase;
                }

                rb.velocity = new Vector2(playerDirection * _chaseSpeed, rb.velocity.y);

            }
            else
            {
                //chase ended, node returns
                Debug.Log("Chase ended");
                rb.velocity -= new Vector2(1f, 1f);
                return Node.Status.Success;
            }

            return Node.Status.Running;
        }

        public void Reset()
        {
            _stopChaseTimer = _timeToStopChase;
        }
    }

    public class RollStrategy : IStrategy
    {
        private KappaAI _enemyAI;
        private Transform _enemyTransform;
        private Transform _playerTransform;
        private float _goalDirection;
        private Vector2 _goalPosition;
        private float _rollingSpeed;
        private float _rollingDistance;
        private float _startDuration;
        private float _startDurationTimer;
        private int _rollDamage;

        private bool _isRollCalculated;

        private Animator _animator;
        private Rigidbody2D _rb;

        public RollStrategy(GameObject enemy, GameObject player, float rollingSpeed, float rollingDistance, float startDuration, int rollDamage)
        {
            _enemyAI = enemy.GetComponent<KappaAI>();
            _enemyTransform = enemy.transform;
            _playerTransform = player.transform;
            _rollingSpeed = rollingSpeed;
            _rollingDistance = rollingDistance;
            _rollDamage = rollDamage;

            _startDuration = startDuration;
            _startDurationTimer = _startDuration;

            _rb = enemy.GetComponent<Rigidbody2D>();
            _animator = enemy.GetComponentInChildren<Animator>();
        }

        public Node.Status Process()
        {
            // calculate position to roll towards
            if (!_isRollCalculated)
            {
                Vector2 enemyPosition = _enemyTransform.position;
                _goalDirection = Mathf.Sign(_playerTransform.position.x - enemyPosition.x);
                _goalPosition =
                    new Vector2(enemyPosition.x + (_rollingDistance * _goalDirection), enemyPosition.y);
                _isRollCalculated = true;
                _animator.SetTrigger("roll");
            }

            // if it's starting the attack slow down
            if (_startDurationTimer > 0)
            {
                _rb.velocity = new Vector2(_goalDirection * (_rollingSpeed / 8), _rb.velocity.y);
                _startDurationTimer -= Time.deltaTime;
                return Node.Status.Running;
            }

            if (Mathf.Abs(_goalPosition.x - _enemyTransform.position.x) <= 0.15f)
            {
                _rb.velocity = Vector2.zero;
                _animator.SetTrigger("stopRoll");
                return Node.Status.Success;
            }
            int groundLayer = LayerMask.NameToLayer("Ground");
            int decorationLayer = LayerMask.NameToLayer("Decoration");
            Collider2D[] entities = Physics2D.OverlapCircleAll(_enemyAI.CollisionDetectionCenter.position, _enemyAI.CollisionDetectionRadius);

            foreach (var e in entities)
            {
                if (!e.CompareTag("Enemy") && e.gameObject.layer != decorationLayer)
                {

                    if (e.CompareTag("Player"))
                    {
                        if (e.transform.parent.TryGetComponent(out PlayerHealth playerHealth))
                        {
                            if (e.transform.parent.TryGetComponent(out PlayerMovement playerMovement))
                            {
                                if (!playerMovement.dashInvulnerability)
                                {
                                    int xDirection = (int)Mathf.Sign(e.transform.position.x - _enemyTransform.position.x);
                                    playerHealth.Damage(_rollDamage, xDirection);
                                    _rb.velocity = Vector2.zero;
                                    _animator.SetTrigger("impact");
                                    return Node.Status.Success;
                                }
                            }
                        }

                        return Node.Status.Running;
                    }
                    if (e.gameObject.layer != groundLayer)
                        return Node.Status.Running;
                        
                    _rb.velocity = Vector2.zero;
                    _animator.SetTrigger("impact");
                    return Node.Status.Success;
                }
            }

            if (_goalDirection > 0 && _enemyTransform.localScale.x > 0)
            {
                _enemyTransform.localScale = new Vector2(-_enemyTransform.localScale.x, _enemyTransform.localScale.y);
            }
            else if (_goalDirection < 0 && _enemyTransform.localScale.x < 0)
            {
                _enemyTransform.localScale = new Vector2(-_enemyTransform.localScale.x, _enemyTransform.localScale.y);
            }

            _rb.velocity = new Vector2(_goalDirection * _rollingSpeed, _rb.velocity.y);

            return Node.Status.Running;
        }

        public void Reset()
        {
            _isRollCalculated = false;
            _startDurationTimer = _startDuration;
        }
    }
}
