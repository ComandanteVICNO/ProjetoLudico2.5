using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyLogicManager : LogicMachineManager
{
    [Header("References")]
    public Transform pointA;
    public Transform pointB;
    public Transform attackPlayerTransform;
    public Transform playerTransform;
    public Rigidbody rb;
    public Transform enemyTransform;
    public DetectPlayerHitbox detectPlayerHitbox;
    public DetectPlayerHitbox detectPlayerSphere;
    public SmallEnemyHealth enemyHealth;
    public PlayerHealth playerHealth;
    public Vector3 initialPlayerPosition;


    [Header("Raycast")]
    public float RaycastDistance;
    public LayerMask playerLayer;

    [Header("Patrol Values")]
    public float minWaitTime;
    public float maxWaitTime;
    public float patrolSpeed;
    public float minDistanceToPoint;

    [Header("Charge Values")]
    public float lungeChargeTime;

    [Header("Lunge Values")]
    public float lungeForce;
    public float lungeDuration;
    public Vector3 directionToPlayer;

    [Header("Stunned Values")]
    public float timeKnockback;
    public float timeStunned;
    public float knockbackForce;

    [Header("RangedState")]
    public GameObject projectile;
    public float projectileCooldown;
    public float projectileForce;

    [Header("Sprite Animator")]
    public Animator spriteAnimator;


    public override void OnAwake()
    {
        enemyHealth = GetComponent<SmallEnemyHealth>();
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        playerTransform = playerHealth.transform;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackPlayerTransform = detectPlayerHitbox.playerTransform;

        if(attackPlayerTransform != null )
        {
            directionToPlayer = attackPlayerTransform.position - transform.position;
            directionToPlayer.Normalize();
            directionToPlayer = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);
        }

        CheckIfInLineOfPlayer();
        
    }

    public bool CheckIfInLineOfPlayer()
    {
        Ray ray = new Ray(transform.position, playerTransform.position);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, Mathf.Infinity, playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }

}
