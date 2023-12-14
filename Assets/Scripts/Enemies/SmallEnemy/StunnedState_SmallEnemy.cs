using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StunnedState_SmallEnemy : LogicMachineBehaviour<SmallEnemyLogicManager>
{
    

    public override void OnAwake()
    {

    }

    public override void OnEnter()
    {
        manager.spriteAnimator.SetBool("isDamaged", true);
        if (manager.enemyHealth.isStunned)
        {
            DoStunnedKnockback(manager.knockbackForce, manager.playerTransform);
        }
        else
        {
            DoKnockback(manager.knockbackForce, manager.playerTransform);
        }
    }
    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        manager.spriteAnimator.SetBool("isDamaged", false);
    }

    public async void DoKnockback(float knockbackForce, Transform playerTransform)
    {
       

        Vector3 direction = (transform.position - playerTransform.position).normalized;
        manager.rb.AddForce(direction * (knockbackForce * 2), ForceMode.Impulse);

        float time = manager.timeKnockback;

        await Task.Delay(TimeSpan.FromSeconds(time));

        if (!active) return;
        manager.enemyHealth.wasAttacked = false;
        logicAnimator.SetBool("wasAttacked", false);
    }
    public async void DoStunnedKnockback(float knockbackForce, Transform playerTransform)
    {
        Vector3 direction = (transform.position - playerTransform.position).normalized;
        manager.rb.AddForce(direction * (knockbackForce * 2), ForceMode.Impulse);

        float time = manager.timeStunned;

        await Task.Delay(TimeSpan.FromSeconds(time));

        if (!active) return;
        manager.enemyHealth.wasAttacked = false;
        logicAnimator.SetBool("wasAttacked", false);


    }
}
