using System.Collections;
using Enemies.BehaviourTree;
using Health;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.Kappa
{
    public class RangeKappaAI : BaseEnemy
    {
        [Header("Chase parameters")]
        [Tooltip("Radius where the player will be detected")]
        [SerializeField] private float detectionRadius;
        [Tooltip("Speed at which the kappa will roll until the player is in it attack radius")]
        [SerializeField] private float rollingSpeed;
        [Tooltip("Time the enemy is stunned on getting hit")]
        [SerializeField] private float onHitStunTime;
        [Tooltip("Strength of the knockback applied to the enemy on hit")]
        [SerializeField] private float knockbackStrength;
        
        [Header("Range parameters")]
        [Tooltip("Radius where the player will be attacked (should be smaller than detectionRadius)")]
        [SerializeField] private float attackRangeRadius;
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float projectileLifetime;
        
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
        private static readonly int RangeAttackHash = Animator.StringToHash("rangeAttack");
        private static readonly int IsRollingHash = Animator.StringToHash("isRolling");

        protected override void Awake()
        {
            base.Awake();
            
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            _entityHealth = GetComponent<EntityHealth>();
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }
        

        protected override void OnDisable()
        {
            base.OnDisable();
            _entityHealth.RemoveListenerOnHit(OnHitKnockback);
        }

        public override void SetUpBehaviourTree()
        {
            _entityHealth.AddListenerOnHit(OnHitKnockback);
            _entityHealth.AddListenerDeathEvent(() => EnemyDie());
            
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
            
            Leaf isPlayerInAttackRangeRadius =
                new Leaf("IsPlayerInAttackRangeRadius", new ConditionStrategy(IsPlayerInAttackRangeRadius));
            Leaf rangeAttack = new Leaf("RangeAttackPlayer", new ActionStrategy(StartRangeAttack));
            Sequence rangeAttackPlayer = new Sequence("RangeAttack");
            rangeAttackPlayer.AddChild(isPlayerInAttackRangeRadius);
            rangeAttackPlayer.AddChild(rangeAttack);
            
            actionToDo.AddChild(attackPlayer);
            actionToDo.AddChild(rangeAttackPlayer);
            actionToDo.AddChild(chasePlayer);
            
            _kappaBehaviourTree.AddChild(actionToDo);
        }
    
        void Update()
        {
            if (!_isEnemyDead) _kappaBehaviourTree.Process();
        }
        
        void MoveTowardsPlayer()
        {
            float xDifference = player.transform.position.x - transform.position.x;
            float playerDirection = Mathf.Sign(xDifference);
            
            if (Mathf.Abs(xDifference) < attackRangeRadius)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                _animator.SetBool(IsRollingHash, false);
            }
            else
            {
                FaceTowardsPlayer(playerDirection);
                _rb.velocity = new Vector2(playerDirection * rollingSpeed, _rb.velocity.y);
                _animator.SetBool(IsRollingHash, true);
            }
                
        }

        void FaceTowardsPlayer(float playerDirection)
        {
            if (playerDirection > 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            } else if (playerDirection < 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
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
        
        void StartRangeAttack()
        {
            _animator.SetTrigger(RangeAttackHash);
            _animator.SetBool(IsRollingHash, false);
            _kappaBehaviourTree.Pause();
        }

        void RangeAttack()
        {
            GameObject instance = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.Euler(Vector3.zero));
            KappaProjectile kappaProjectileInstance = instance.GetComponent<KappaProjectile>();
            float xDifference = player.transform.position.x - transform.position.x;
            float playerDirection = Mathf.Sign(xDifference);
            FaceTowardsPlayer(playerDirection);
            kappaProjectileInstance.Speed = playerDirection * projectileSpeed;
            kappaProjectileInstance.Lifetime = projectileLifetime;
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
            return Vector3.Distance(transform.position, player.transform.position) <= detectionRadius;
        }
        
        private bool IsPlayerInAttackRangeRadius()
        {
            float xDifference = player.transform.position.x - transform.position.x;
            return Mathf.Abs(xDifference) <= attackRangeRadius;
        }
        
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRangeRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackCenter.position, attackRadius);
        }
    
    }
}
