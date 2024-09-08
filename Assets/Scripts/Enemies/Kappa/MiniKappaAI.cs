using System.Collections;
using Enemies.BehaviourTree;
using Health;
using UnityEngine;

namespace Enemies.Kappa
{
    public class MiniKappaAI : MonoBehaviour
    {
        [Header("Reference to the player")]
        [SerializeField] private GameObject player;
   
        [Header("Chase parameters")]
        [Tooltip("Radius where the player will be detected")]
        [SerializeField] private float detectionRadius;
        [SerializeField] private float chaseSpeed;
        [Tooltip("Time the enemy is stunned on getting hit")]
        [SerializeField] private float onHitStunTime;
        [Tooltip("Strength of the knockback applied to the enemy on hit")]
        [SerializeField] private float knockbackStrength;
        
        [Header("Attack parameters")]
        [Tooltip("Center of the attack radius")]
        [SerializeField] private Transform attackCenter;
        [Tooltip("Radius of the attack")]
        [SerializeField] private float attackRadius;
        [SerializeField] private int attackDamage;
        [SerializeField] private LayerMask hurtboxLayer;
    
        private BehaviourTree.BehaviourTree _miniKappaBehaviourTree;
        private EntityHealth _entityHealth;
        
        private Rigidbody2D _rb;
        private Animator _animator;
        private static readonly int AttackHash = Animator.StringToHash("attack");
        private static readonly int IsMovingHash = Animator.StringToHash("isMoving");

        private void Awake()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            _entityHealth = GetComponent<EntityHealth>();
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            
        }

        private void OnDisable()
        {
            _entityHealth.RemoveListenerDeathEvent(() => Destroy(gameObject));
            
        }

        private void Start()
        {
        
            _entityHealth.AddListenerDeathEvent(() => Destroy(gameObject));
            _entityHealth.AddListenerOnHit(OnHitKnockback);
        
            _miniKappaBehaviourTree = new BehaviourTree.BehaviourTree("MiniKappa", Policies.RunForever);
        
            Leaf isPlayerInDetectionRadius =
                new Leaf("IsPlayerInDetectionRadius", new ConditionStrategy(IsPlayerDetected));
            Leaf moveToPlayer = new Leaf("MoveToPlayer", new ActionStrategy(MoveTowardsPlayer));

            Selector actionToDo = new Selector("ActionToDo");

            Leaf isPlayerClose = new Leaf("Attack", new ConditionStrategy(IsPlayerInMeleeRange));
            Leaf attack = new Leaf("Attack", new ActionStrategy(StartMeleeAttack));
            Sequence attackPlayer = new Sequence("AttackPlayer");
            attackPlayer.AddChild(isPlayerClose);
            attackPlayer.AddChild(attack);
            actionToDo.AddChild(attackPlayer);
        
            Sequence chasePlayer = new Sequence("ChasePlayer");
            chasePlayer.AddChild(isPlayerInDetectionRadius);
            chasePlayer.AddChild(moveToPlayer);
            actionToDo.AddChild(chasePlayer);
            
            _miniKappaBehaviourTree.AddChild(actionToDo);
        }
    
        private void Update()
        {
            _miniKappaBehaviourTree.Process();
        }
        
        private void MoveTowardsPlayer()
        {
            float xDifference = player.transform.position.x - transform.position.x;
            float playerDirection = Mathf.Sign(xDifference);
            
            if (Mathf.Abs(xDifference) < 0.2)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                _animator.SetBool(IsMovingHash, false);
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
                _rb.velocity = new Vector2(playerDirection * chaseSpeed, _rb.velocity.y);
                _animator.SetBool(IsMovingHash, true);
            }
                
        }

        private bool IsPlayerInMeleeRange()
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
        
        private void StartMeleeAttack()
        {
            _rb.velocity = Vector2.zero;
            _miniKappaBehaviourTree.Pause();
            _animator.SetTrigger(AttackHash);
        }
        
        private void MeleeAttackCollision()
        {
            Collider2D[] entities = Physics2D.OverlapCircleAll(attackCenter.position, attackRadius, hurtboxLayer);
            foreach (var entity in entities)
            {
                if (entity.CompareTag("Player"))
                {
                    if (entity.transform.root.TryGetComponent(out PlayerHealth playerHealth))
                    {
                        int xDirection = (int) Mathf.Sign(entity.transform.position.x - transform.position.x);
                        playerHealth.Damage(attackDamage, xDirection);
                    }
                }
            }
        }

        private void Unpause()
        {
            _miniKappaBehaviourTree.Unpause();
        }

        private void OnHitKnockback()
        {
            _rb.velocity = Vector2.zero;
            Vector2 knockbackDirection = transform.position - player.transform.position;
            _rb.AddForce(knockbackDirection.normalized * knockbackStrength, ForceMode2D.Impulse);
            if (onHitStunTime > 0)
                StartCoroutine(StunEnemy(onHitStunTime));
        }

        private IEnumerator StunEnemy(float time)
        {
            _miniKappaBehaviourTree.Pause();
            yield return new WaitForSeconds(time);
            _miniKappaBehaviourTree.Unpause();
        }
    
        private bool IsPlayerDetected()
        {
            return Vector3.Distance(transform.position, player.transform.position) <= detectionRadius;
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackCenter.position, attackRadius);
        }
    
    }
}
