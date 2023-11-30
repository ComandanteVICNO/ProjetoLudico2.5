using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NormalEnemyLogicManager : LogicMachineManager
{
    [Header("References")]
    public Transform pointA;
    public Transform pointB;
    public Transform enemyTransform;
    public Transform playerTransform;
    public Rigidbody rb;
    public PlayerMovController playerMovController;
    public ChaseDetectPlayer chaseHitbox;
    public AttackDetectPlayer attackHitbox;
    public PlayerHealth playerHealth;
    public EnemyHealth enemyHealth;

    [Header("Values")]
    public float minWaitTime;
    public float maxWaitTime;
    public float patrolSpeed;
    public float chaseSpeed;
    public float minDistanceToPoint;
    public float attackCooldown;
    public float damageValue;
    public float timeStunned;
    public float timeknockback;
    public float knockbackForce;
    


    public override void OnAwake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        playerMovController = FindObjectOfType<PlayerMovController>();
        playerTransform = playerMovController.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    
}
