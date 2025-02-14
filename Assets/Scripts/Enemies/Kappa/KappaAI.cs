using System.Collections;
using Enemies.BehaviourTree;
using Health;
using PlayerController;
using UnityEngine;
using Utils;

namespace Enemies.Kappa
{
    public class KappaAI : BaseEnemy, IResettable
    {
        [Header("Chase parameters")]
        [Tooltip("Radius where the player will be detected")]
        [SerializeField] private float detectionRadius;
        [SerializeField] private float chaseSpeed;
        [Tooltip("Time the enemy is stunned on getting hit")]
        [SerializeField] private float onHitStunTime;
        [Tooltip("Strength of the knockback applied to the enemy on hit")]
        [SerializeField] private float knockbackStrength;
        
        [Header("Roll parameters")]
        [Tooltip("Speed of the roll")]
        [SerializeField] private float rollingSpeed;
        [Tooltip("Distance that the kappa rolls")]
        [SerializeField] private float rollingDistance;
        [Tooltip("Center of the collision detection radius")]
        [SerializeField] private Transform collisionDetectionCenter;
        [Tooltip("Radius of the attack")]
        [SerializeField] private float collisionDetectionRadius;
        [Tooltip("Contact damage of the roll")]
        [SerializeField] private int rollDamage;
        [Space]
        [Tooltip("Time of the animation that the kappa will move slowly at (to match the jumping animation)")]
        [SerializeField] private float _rollAttackStartDuration = 0.84375f;

        public Transform CollisionDetectionCenter => collisionDetectionCenter;

        public float CollisionDetectionRadius => collisionDetectionRadius;

        [Header("Attack parameters")]
        [Tooltip("Center of the attack radius")]
        [SerializeField] private Transform attackCenter;
        [Tooltip("Radius of the attack")]
        [SerializeField] private float attackRadius;
        [SerializeField] private int attackDamage;
        [SerializeField] private LayerMask hurtboxLayer;
    
        private BehaviourTree.BehaviourTree _kappaBehaviourTree;
        
        private Rigidbody2D _rb;
        private Animator _animator;
        private static readonly int AttackHash = Animator.StringToHash("attack");
        private static readonly int IsWalkingHash = Animator.StringToHash("walk");
        private static readonly int IdleHash = Animator.StringToHash("idle");
        private static readonly int HitHash = Animator.StringToHash("hit");

        [SerializeField] public GameObject kappaShell;

        private bool _isRolling;
        private bool _isChasingPlayer;
        private bool _isDetected;

        private Vector3 _initialPos;

        protected override void Awake()
        {
            base.Awake();
            
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponentInChildren<Animator>();

            _initialPos = transform.position;
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _entityHealth.AddListenerOnHit(OnHitKnockback);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _entityHealth.RemoveListenerOnHit(OnHitKnockback);
        }

        public override void SetUpBehaviourTree()
        {
            _entityHealth.AddListenerOnHit(OnHitKnockback);
            
            _kappaBehaviourTree = new BehaviourTree.BehaviourTree("Kappa", Policies.RunForever);

            Selector actionToDo = new Selector("ActionToDo");

            Leaf isPlayerClose = new Leaf("Attack", new ConditionStrategy(IsPlayerInMeleeRange));
            Leaf attack = new Leaf("Attack", new ActionStrategy(StartMeleeAttack));
            Sequence attackPlayer = new Sequence("AttackPlayer");
            attackPlayer.AddChild(isPlayerClose);
            attackPlayer.AddChild(attack);
            
            Leaf isPlayerInDetectionRadius =
                new Leaf("IsPlayerInDetectionRadius", new ConditionStrategy(IsPlayerDetected));
            Leaf moveToPlayer = new Leaf("MoveToPlayer", new ActionStrategy(MoveTowardsPlayer));
            Sequence chasePlayer = new Sequence("ChasePlayer");
            chasePlayer.AddChild(isPlayerInDetectionRadius);
            chasePlayer.AddChild(moveToPlayer);
            
            Leaf didPlayerEnterDetectionRadius =
                new Leaf("DidPlayerEnterDetectionRadius", new ConditionStrategy(IsPlayerDetectedFirstTime));
            Leaf roll = new Leaf("RollTowardsPlayer", new RollStrategy(gameObject, player, rollingSpeed, rollingDistance,
                _rollAttackStartDuration, rollDamage));
            Sequence rollToPlayer = new Sequence("Roll");
            rollToPlayer.AddChild(didPlayerEnterDetectionRadius);
            rollToPlayer.AddChild(roll);
            
            actionToDo.AddChild(rollToPlayer);
            actionToDo.AddChild(attackPlayer);
            actionToDo.AddChild(chasePlayer);
            
            _kappaBehaviourTree.AddChild(actionToDo);
        }
    
        void Update()
        {
            if (!_isEnemyDead && _kappaBehaviourTree != null)
            {
                _kappaBehaviourTree.Process();
            }
        }
        
        void MoveTowardsPlayer()
        {
            float xDifference = player.transform.position.x - transform.position.x;
            float playerDirection = Mathf.Sign(xDifference);
            
            if (Mathf.Abs(xDifference) < 0.2)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                _animator.SetTrigger(IdleHash);
            }
            else
            {
                if (playerDirection > 0 && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                } else if (playerDirection < 0 && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                }
                
                Collider2D[] entities = Physics2D.OverlapCircleAll(collisionDetectionCenter.position, collisionDetectionRadius,
                    LayerMask.GetMask("Ground"));
                if (entities.Length > 0)
                {
                    _rb.velocity = new Vector2(0, _rb.velocity.y);
                    _animator.SetTrigger(IdleHash);
                }
                else
                {
                    _rb.velocity = new Vector2(playerDirection * chaseSpeed, _rb.velocity.y);
                    _animator.SetTrigger(IsWalkingHash);
                }
            }
            
        }

        bool IsPlayerInMeleeRange()
        {
            Collider2D[] entities = Physics2D.OverlapCircleAll(attackCenter.position, attackRadius, hurtboxLayer);
            foreach (var entity in entities)
            {
                if (entity.CompareTag("Player"))
                {
                    return true;
                }
            }

            return false;
        }
        
        void StartMeleeAttack()
        {
            _rb.velocity = Vector2.zero;
            _kappaBehaviourTree.Pause();
            _animator.SetTrigger(AttackHash);
        }
        
        void MeleeAttackCollision()
        {
            Collider2D[] entities = Physics2D.OverlapCircleAll(attackCenter.position, attackRadius, hurtboxLayer);
            foreach (var entity in entities)
            {
                if (entity.CompareTag("Player"))
                {
                    if (entity.transform.parent.TryGetComponent(out PlayerHealth playerHealth))
                    {
                        int xDirection = (int) Mathf.Sign(entity.transform.position.x - transform.position.x);
                        playerHealth.Damage(attackDamage, xDirection);
                    }
                }
            }
        }

        void Unpause()
        {
            _kappaBehaviourTree.Unpause();
        }

        private void OnHitKnockback()
        {
            _rb.velocity = Vector2.zero;
            Vector2 knockbackDirection = transform.position - player.transform.position;
            _rb.AddForce(knockbackDirection.normalized * knockbackStrength, ForceMode2D.Impulse);
            _animator.SetTrigger(HitHash);
            if (onHitStunTime > 0)
                StartCoroutine(StunEnemy(onHitStunTime));
        }

        IEnumerator StunEnemy(float time)
        {
            _kappaBehaviourTree.Pause();
            yield return new WaitForSeconds(time);
            _kappaBehaviourTree.Unpause();
        }
    
        private bool IsPlayerDetected()
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
            {

                if (_isDetected)
                {
                    _entityHealth.damageable = true;
                    _entityHealth.giveUpwardForce = false;
                    return true;
                }
            }
            
            _isDetected = false;
            _animator.SetTrigger(IdleHash);
            return false;
        }
        
        private bool IsPlayerDetectedFirstTime()
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
            {

                if (!_isDetected)
                {
                    _isDetected = true;
                    _entityHealth.damageable = false;
                    _entityHealth.giveUpwardForce = true;
                    return true;
                }
            }
            
            return false;
        }
        
        protected override void EnemyDie()
        {
            base.EnemyDie();
            StartCoroutine(DisableEnemy());

        }

        private IEnumerator DisableEnemy()
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(kappaShell, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackCenter.position, attackRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(collisionDetectionCenter.position, collisionDetectionRadius);
        }


        public void ResetObject()
        {
            transform.position = _initialPos;
            gameObject.SetActive(true);
        }
    }
}
