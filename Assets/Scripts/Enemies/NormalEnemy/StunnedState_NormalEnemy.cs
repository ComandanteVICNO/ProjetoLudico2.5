using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class StunnedState_NormalEnemy : LogicMachineBehaviour<NormalEnemyLogicManager>
{


    public override void OnAwake()
    {
        
    }

    public override void OnEnter()
    {
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
        
    }

    public async void DoKnockback(float knockbackForce, Transform playerTransform)
    {
        //Vector3 direction = (transform.position - playerTransform.position).normalized;
        //manager.rb.AddForce(direction * knockbackForce, ForceMode.Impulse);

        //manager.enemyHealth.wasAttacked = false;
        //logicAnimator.SetBool("wasAttacked", false);

        Vector3 direction = (transform.position - playerTransform.position).normalized;
        manager.rb.AddForce(direction * (knockbackForce * 2), ForceMode.Impulse);

        float time = manager.timeknockback;

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
