using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AttackState : LogicMachineBehaviour<NormalEnemyLogicManager>
{
    bool canAttack;
    
    public override void OnAwake()
    {
        
    }

    public override void OnEnter()
    {
        canAttack = true;
    }
    public override void OnUpdate()
    {
        if (!manager.attackHitbox.isPlayerDetected)
        {
            logicAnimator.SetBool("canAttackPlayer", false);
        }
        if (manager.chaseHitbox.isPlayerDetected)
        {
            logicAnimator.SetBool("canChasePlayer", true);
        }
        if (manager.enemyHealth.wasAttacked)
        {
            logicAnimator.SetBool("isStunned", true);
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

    async void DoAttack()
    {
        canAttack = false;

        //Do attack func here:
        manager.playerHealth.TakeDamage(manager.damageValue);


        float attackCooldown = manager.attackCooldown;
        await Task.Delay(TimeSpan.FromSeconds(attackCooldown));

        if (!active) return;
        canAttack = true;

    }


    void StopMovement()
    {
        manager.rb.velocity = Vector3.zero;
    }
}
