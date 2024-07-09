using System.Collections;
using System.Collections.Generic;
using Enemies;
using Enemies.BehaviourTree;
using UnityEditor;
using UnityEngine;

public class ChaserAI : PlayerDetectorEnemy
{
    [Header("Reference to the player")]
    [SerializeField] private GameObject player;
   
    [Header("Chase parameters")]
    [Tooltip("Radius where the player will be detected")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private float chaseSpeed;
    [Tooltip("Time to stop the chase once the player is out of the radius")]
    [SerializeField] private float timeToStopChase;
    
    [Header("Player above detection area parameters")]
    [SerializeField] private float areaWidth;
    [SerializeField] private float areaHeigth;
    
    [Header("Smart jump parameters")]
    public LayerMask groundLayer;
    [SerializeField] private float jumpForce;

    
    private BehaviourTree _chaserBehaviourTree;
    
    void Start()
    {
        _chaserBehaviourTree = new BehaviourTree("Chaser", Policies.RunForever);
        
        Leaf isPlayerInDetectionRadius =
            new Leaf("IsPlayerInDetectionRadius", new ConditionStrategy(IsPlayerDetected));
        Leaf moveToPlayer = new Leaf("MoveToPlayer", new ChaseStrategy(gameObject, player, detectionRadius,
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
