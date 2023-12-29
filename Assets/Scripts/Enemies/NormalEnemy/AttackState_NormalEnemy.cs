using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AttackState_NormalEnemy : LogicMachineBehaviour<NormalEnemyLogicManager>
{
    bool canAttack;
    public bool isAttacking;
    public bool isWaitingCooldown;

    


    public float currentAttackCooldown;
    float currentTimeUntilAttackHits;
    float currentTimeUntilAnimationStops;
    public override void OnAwake()
    {
        
    }

    public override void OnEnter()
    {
        currentTimeUntilAttackHits = manager.timeUntilAttackHits;
        currentAttackCooldown = manager.animationTime - manager.timeUntilAttackHits;
        
        canAttack = true;
        isAttacking = false;
        isWaitingCooldown = false;
        
        logicAnimator.SetBool("canChasePlayer", false);
    }
    public override void OnUpdate()
    {
        if (!isAttacking)
        {
            if (!manager.attackHitbox.isPlayerDetected)
            {
                logicAnimator.SetBool("canAttackPlayer", false);
            }
            
        
        }
        if (manager.enemyHealth.wasAttacked)
        {
            logicAnimator.SetBool("wasAttacked", true);
        }

        StopMovement();
       
        
        if(canAttack)
        {
            Attack();
        }
        else
        {
            AttackCooldown();

        }
        
        

    }

    public override void OnExit()
    {
        
        logicAnimator.SetBool("canAttackPlayer", false);
        manager.animator.SetBool("isAttacking", false);
    }

    
    
    void Attack()
    {
        manager.animator.SetBool("isAttacking", true);
        
        isAttacking = true;
        currentTimeUntilAttackHits -= Time.deltaTime;
        if(currentTimeUntilAttackHits <= 0 && canAttack)
        {
            
            canAttack = false;
            if (manager.attackHitbox.isPlayerDetected)
            {
                manager.playerHealth.TakeDamage(manager.damageValue);
            }
        }
        
        

    }

    void AttackCooldown()
    {
        currentAttackCooldown -= Time.deltaTime;
        if(currentAttackCooldown <= 0)
        {
            currentTimeUntilAttackHits = manager.timeUntilAttackHits;
            currentAttackCooldown = manager.animationTime - manager.timeUntilAttackHits;
            isAttacking = false;
            canAttack = true;
            
            manager.animator.SetBool("isAttacking", false);
        }
    }

 


    void StopMovement()
    {
        manager.rb.velocity = Vector3.zero;
    }
}
