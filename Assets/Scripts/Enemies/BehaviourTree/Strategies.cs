using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

    public class PatrolStrategy : IStrategy
    {
        private GameObject _enemy;
        private Transform _enemyTransform;
        private List<Transform> _patrolPoints;
        private float _waitTime;
        private float _waitTimer;
        private float _patrolSpeed;
        private int _currentPoint;

        public PatrolStrategy(GameObject enemy, List<Transform> patrolPoints, float waitTime, float patrolSpeed = 5f)
        {
            _enemy = enemy;
            _enemyTransform = enemy.transform;
            _patrolPoints = patrolPoints;
            _waitTime = waitTime;
            _patrolSpeed = patrolSpeed;
            _waitTimer = _waitTime;
        }
        
        public Node.Status Process()
        {
            DetectorEnemy detector = _enemy.GetComponent<DetectorEnemy>();
            if (detector.IsPlayerDetected()) return Node.Status.Success;
            if (_currentPoint == _patrolPoints.Count) return Node.Status.Success;
            Transform targetPoint = _patrolPoints[_currentPoint];
            float sign = Mathf.Sign(targetPoint.position.x - _enemyTransform.position.x);
            detector.ChangeDirection(sign);
            
            if (Vector3.Distance(_enemyTransform.transform.position, targetPoint.transform.position) > 0.5f)
            {
                _enemyTransform.position = Vector3.MoveTowards(_enemyTransform.transform.position,
                    new Vector3(targetPoint.position.x, _enemyTransform.position.y, 0), _patrolSpeed * Time.deltaTime);
            }
            else
            {
                _waitTimer -= Time.deltaTime;
                if (_waitTimer <= 0)
                {
                    _currentPoint++;
                    _waitTimer = _waitTime;
                }
            }
            
            return Node.Status.Running;
        }

        public void Reset()
        {
            _currentPoint = 0;
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
        
        private LayerMask _groundLayer;
        private float _jumpForce;
        private bool _isGrounded;
        private bool _shouldJump;

        private float _areaWidth;
        private float _areaHeight;
        
        

        public ChaseStrategy(GameObject enemy, GameObject player, float detectionRadius, float chaseSpeed,
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
}
