using System.Collections;
using System.Collections.Generic;
using Enemies.BehaviourTree;
using Health;
using UnityEditor;
using UnityEngine;

namespace Enemies
{
    public class KappaAI : PlayerDetectorEnemy
    {
        [Header("Reference to the player")]
        [SerializeField] private GameObject player;
    
        [Header("Patrol parameters")]
        [Tooltip("Ordered points the enemy will traverse through")]
        [SerializeField] private List<Transform> patrolPoints = new();
        [Tooltip("Time the enemy will stop in each point")]
        [SerializeField] private float patrolWaitTime;
        [SerializeField] private float patrolSpeed;

        [Header("Chase parameters")]    
        [Tooltip("Radius where the player will be detected")]
        [SerializeField] private float detectionAreaPositionX;
        [SerializeField] private float detectionAreaPositionY;
        [SerializeField] private float detectionAreaWidth;
        [SerializeField] private float detectionAreaHeight;
        [SerializeField] private float meleeDistance;
        [SerializeField] private float chaseSpeed;
    
        private BehaviourTree.BehaviourTree _kappaBehaviourTree;

        private Animator _animator;

        private bool _isAttacking;
    
        void Start()
        {
            _animator = GetComponent<Animator>();
            
            // remember that the order in which you add the nodes is important
            _kappaBehaviourTree = new BehaviourTree.BehaviourTree("Kappa");
        
            Selector combatOrPatrol = new Selector("CombatOrPatrol");
        
            Sequence combat = new Sequence("Combat");
            Leaf isPlayerDetected = new Leaf("IsPlayerDetected", new ConditionStrategy(IsPlayerDetected));
            combat.AddChild(isPlayerDetected);
            
            Selector attackActionToDo = new Selector("AttackActionToDo");
                
            Sequence attackPlayerIfClose = new Sequence("AttackPlayerIfClose");
            Leaf isPlayerClose = new Leaf("IsPlayerDetected", new ConditionStrategy(IsPlayerClose));
            Leaf meleeSwipe = new Leaf("MeleeSwipe", new ActionStrategy(MeleeSwipe));
            attackPlayerIfClose.AddChild(isPlayerClose);
            attackPlayerIfClose.AddChild(meleeSwipe);
                    
            Selector attackPlayerIfFar = new Selector("AttackPlayerIfFar");
                    
            Sequence lowHealth = new Sequence("LowHealth");
            Leaf isHealthLow = new Leaf("IsHealthLow", new ConditionStrategy(IsLowHealth));
            Leaf rangedAttackHealthLow = new Leaf("RangedAttackHealthLow", new ActionStrategy(RangedAttack));
            Leaf backAwayFromPlayer = new Leaf("BackAwayFromPlayer", new ActionStrategy(BackAwayFromPlayer));
            lowHealth.AddChild(isHealthLow);
            lowHealth.AddChild(rangedAttackHealthLow);
            lowHealth.AddChild(backAwayFromPlayer);

            Sequence highHealth = new Sequence("HighHealth");
            Leaf rangedAttack = new Leaf("RangedAttack", new ActionStrategy(RangedAttack));
            Leaf getCloseToPlayer = new Leaf("GetCloseToPlayer", new ActionStrategy(GetCloseToPlayer));
            highHealth.AddChild(rangedAttack);
            highHealth.AddChild(getCloseToPlayer);
                        
            attackPlayerIfFar.AddChild(lowHealth);
            attackPlayerIfFar.AddChild(highHealth);
                
            attackActionToDo.AddChild(attackPlayerIfClose);
            attackActionToDo.AddChild(attackPlayerIfFar);
            combat.AddChild(attackActionToDo);
        
            Leaf patrol = new Leaf("Patrol", new PatrolStrategy(gameObject, patrolPoints, patrolWaitTime, patrolSpeed));
        
            combatOrPatrol.AddChild(combat);
            combatOrPatrol.AddChild(patrol);
        
            _kappaBehaviourTree.AddChild(combatOrPatrol);
        }
    
        void Update()
        {
            _kappaBehaviourTree.Process();
            // todo: get movement direction and flip detection area x coordinate symbol to face movement direction
        }

        public override bool IsPlayerDetected()
        {
            return Physics2D.OverlapBox(transform.position + new Vector3(detectionAreaPositionX, detectionAreaPositionY),
                new Vector2(detectionAreaWidth, detectionAreaHeight), 0, 1 << player.transform.gameObject.layer);
        }
    
        private bool IsPlayerClose()
        {
            return IsPlayerDetected()
                   && (Vector3.Distance(gameObject.transform.position, player.transform.position) < meleeDistance);
        
        }

        private bool IsLowHealth()
        {
            if (gameObject.TryGetComponent(out EntityHealth entityHealth))
            {
                return entityHealth.GetHealthPercent() < 0.3f;
            }

            return true;
        }

        public override void ChangeDirection(float sign)
        {
            detectionAreaPositionX = Mathf.Abs(detectionAreaPositionX) * sign;
        }

        private void MeleeSwipe()
        {
            if (!_isAttacking)
            {
                Debug.Log("Toma esta");
                _animator.SetTrigger("attack");
                _isAttacking = true;
                StartCoroutine(MeleeCooldown());
            }

        }

        private void RangedAttack()
        {
            Debug.Log("Rango ataque");
        }

        private void BackAwayFromPlayer()
        {
            Debug.Log("I got the triller");
        }
    
        private void GetCloseToPlayer()
        {
            Debug.Log("Voy a por ti");
        }
    
        private void OnDrawGizmosSelected()
        {
            int i = 0;
            foreach (Transform point in patrolPoints)
            {
                Handles.Label(point.position, i.ToString());
                i++;
            }
        
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(detectionAreaPositionX,detectionAreaPositionY),
                new Vector2(detectionAreaWidth, detectionAreaHeight));
        }

        private IEnumerator MeleeCooldown()
        {
            yield return new WaitForSeconds(1f);
            _isAttacking = false;
            _animator.ResetTrigger("attack");
        }
    
    }
}

