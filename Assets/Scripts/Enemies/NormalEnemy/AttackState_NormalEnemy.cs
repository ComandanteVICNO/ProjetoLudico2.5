using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AttackState_NormalEnemy : LogicMachineBehaviour<NormalEnemyLogicManager>
{
    bool canAttack;
    bool isAttacking;
    public float originalAttackCooldown;
    public float currentAttackCooldownM;
    
    public override void OnAwake()
    {
        
    }

    public override void OnEnter()
    {
        originalAttackCooldown = manager.attackCooldown;
        currentAttackCooldownM = originalAttackCooldown;
        canAttack = true;
        isAttacking = false;
    }
    public override void OnUpdate()
    {
        if (!isAttacking)
        {
            if (!manager.attackHitbox.isPlayerDetected)
            {
                logicAnimator.SetBool("canAttackPlayer", false);
            }
            if (manager.chaseHitbox.isPlayerDetected)
            {
                logicAnimator.SetBool("canChasePlayer", true);
            }
        }

        if (manager.enemyHealth.wasAttacked)
        {
            logicAnimator.SetBool("wasAttacked", true);
        }

        StopMovement();
        if (canAttack)
        {
            DoAttack();
        }
        

    }

    public override void OnExit()
    {
        logicAnimator.SetBool("canAttackPlayer", false);
    }

    
    void DoAttack()
    {
        if(canAttack)
        {
            isAttacking = true;
            currentAttackCooldownM -= Time.deltaTime;
            if(currentAttackCooldownM <= 0)
            {
                if (manager.attackHitbox.isPlayerDetected)
                {
                    manager.playerHealth.TakeDamage(manager.damageValue);
                }
                currentAttackCooldownM = originalAttackCooldown;
                isAttacking = false;
            }
        }
    }


    void StopMovement()
    {
        manager.rb.velocity = Vector3.zero;
    }
}
