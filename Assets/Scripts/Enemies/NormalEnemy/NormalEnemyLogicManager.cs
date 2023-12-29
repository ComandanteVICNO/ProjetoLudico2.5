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
    public NormalEnemyHealth enemyHealth;
    public Animator animator;

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

    [Header("Attack Timing Values")]
    public float timeUntilAttackHits;
    public float animationTime;
    public float afterAttack;

    [Header("Animation Clip")]

    public AnimationClip attackAnimation;

    [Header("Step Sounds")]
    public AudioSource footstepAudioSource;
    public AudioClip[] audioSteps;
    public float patrolStepSpeed;
    public float chaseStepSpeed;



    public override void OnAwake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        enemyHealth = GetComponent<NormalEnemyHealth>();
        playerMovController = FindObjectOfType<PlayerMovController>();
        playerTransform = playerMovController.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        animationTime = attackAnimation.length;
        afterAttack = attackAnimation.length - animationTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    
    
}
