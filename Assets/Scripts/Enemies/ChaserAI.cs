using System.Collections;
using Enemies.BehaviourTree;
using Health;
using UnityEngine;

namespace Enemies
{
    public class ChaserAI : DetectorEnemy
    {
        [Header("Reference to the player")]
        [SerializeField] private GameObject player;
   
        [Header("Chase parameters")]
        [Tooltip("Radius where the player will be detected")]
        [SerializeField] private float detectionRadius;
        [SerializeField] private float chaseSpeed;
        [Tooltip("Time to stop the chase once the player is out of the radius")]
        [SerializeField] private float timeToStopChase;
        [Tooltip("Time the enemy is stunned on getting hit")]
        [SerializeField] private float onHitStunTime;
        [Tooltip("Strength of the knockback applied to the enemy on hit")]
        [SerializeField] private float knockbackStrength;
    
        [Header("Player above detection area parameters")]
        [SerializeField] private float areaWidth;
        [SerializeField] private float areaHeigth;
    
        [Header("Smart jump parameters")]
        public LayerMask groundLayer;
        [SerializeField] private float jumpForce;

    
        private BehaviourTree.BehaviourTree _chaserBehaviourTree;
        private EntityHealth _entityHealth;
        private Rigidbody2D _rb;

        void Awake()
        {
            _entityHealth = GetComponent<EntityHealth>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnDisable()
        {
            _entityHealth.RemoveListenerDeathEvent(() => Destroy(gameObject));
            
        }

        void Start()
        {
        
            _entityHealth.AddListenerDeathEvent(() => Destroy(gameObject));
            _entityHealth.AddListenerOnHit(OnHitKnockback);
        
            _chaserBehaviourTree = new BehaviourTree.BehaviourTree("Chaser", Policies.RunForever);
        
            Leaf isPlayerInDetectionRadius =
                new Leaf("IsPlayerInDetectionRadius", new ConditionStrategy(IsPlayerDetected));
            Leaf moveToPlayer = new Leaf("MoveToPlayer", new ChaseStrategyWithJump(gameObject, player, detectionRadius,
                chaseSpeed, timeToStopChase, groundLayer, jumpForce, areaWidth, areaHeigth));
        
            Sequence chasePlayer = new Sequence("ChasePlayer");
            chasePlayer.AddChild(isPlayerInDetectionRadius);
            chasePlayer.AddChild(moveToPlayer);
            _chaserBehaviourTree.AddChild(chasePlayer);
        }
    
        void Update()
        {
            _chaserBehaviourTree.Process();
        }

        private void OnHitKnockback()
        {
            _rb.velocity = Vector2.zero;
            Vector2 knockbackDirection = transform.position - player.transform.position;
            _rb.AddForce(knockbackDirection.normalized * knockbackStrength, ForceMode2D.Impulse);
            if (onHitStunTime > 0)
                StartCoroutine(StunEnemy());
        }

        IEnumerator StunEnemy()
        {
            _chaserBehaviourTree.Pause();
            yield return new WaitForSeconds(onHitStunTime);
            _chaserBehaviourTree.Unpause();
        }
    
        public override bool IsPlayerDetected()
        {
            return Vector3.Distance(transform.position, player.transform.position) <= detectionRadius;
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position + new Vector3(0f,4f,0f), new Vector2(areaWidth, areaHeigth));
        }
    
    }
}
