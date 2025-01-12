using System.Collections;
using Enemies.BehaviourTree;
using Health;
using SaveSystem;
using UnityEngine;
using Utils;
using Utils.CustomLogs;

namespace Enemies.Kappa
{
    public class MiniKappaAI : BaseEnemy, IResettable
    {
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

        private AudioSource _audioSource;
        
        [SerializeField] public GameObject spiderSprite;
        
        private Rigidbody2D _rb;
        private Animator _animator;
        private static readonly int AttackHash = Animator.StringToHash("attack");
        private static readonly int IsMovingHash = Animator.StringToHash("isMoving");

        protected override void Awake()
        {
            base.Awake();
            
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
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

        protected override void LoadState(TemporalDataSO temporalData)
        {
            Destroy(gameObject);
        }

        protected override void SaveState(TemporalDataSO temporalData)
        {
            // intentionally left blank
        }

        private void Update()
        {
            if (!_isEnemyDead) _miniKappaBehaviourTree.Process();
        }
        
        private void MoveTowardsPlayer()
        {
            float xDifference = player.transform.position.x - transform.position.x;
            float playerDirection = Mathf.Sign(xDifference);
            
            if (Mathf.Abs(xDifference) < 0.2)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                _audioSource.Stop();
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
                if (!_audioSource.isPlaying) _audioSource.Play();
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
                    if (entity.transform.parent.TryGetComponent(out PlayerHealth playerHealth))
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
            if (_audioSource.isPlaying) _audioSource.Stop();
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
            if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius) return true;
            if (_audioSource.isPlaying) _audioSource.Stop();
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
            Instantiate(spiderSprite, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackCenter.position, attackRadius);
        }

        public void ResetObject()
        {
            Destroy(gameObject);
        }
    }
}
