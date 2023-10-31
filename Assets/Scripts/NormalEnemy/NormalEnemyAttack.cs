using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyAttack : MonoBehaviour
{
    [Header("References")]
    public DetectPlayer sphereDetectPlayer;
    public Transform enemyAttackPoint;
    public LayerMask playerLayer;

    [Header("Values")]
    public float attackRange;
    public float damage;
    public float attackCooldown;
    private float originalAttackCooldown;

    private bool canAttack;
    private bool isPlayerDetected;

    void Start()
    {
        originalAttackCooldown = attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        AttackCooldown();
    }

    void Attack()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(enemyAttackPoint.position, attackRange, playerLayer);

        foreach (Collider enemy in hitPlayer)
        {
            enemy.GetComponent<PlayerHealth>().TakeDamage(damage);

        }
    }

    void AttackCooldown()
    {
        if (sphereDetectPlayer.PlayerDectionStatus())
        {
            isPlayerDetected = true;
        }
        else
        {
            isPlayerDetected = false;
        }

        if (isPlayerDetected)
        {
            attackCooldown -= Time.deltaTime;
            if(attackCooldown <= 0 && canAttack)
            {
                Debug.Log("Attack");
                Attack();

                canAttack = false;
            }
        }
        else
        {
            attackCooldown = originalAttackCooldown;

            canAttack = true;
        }
        
    }
}
