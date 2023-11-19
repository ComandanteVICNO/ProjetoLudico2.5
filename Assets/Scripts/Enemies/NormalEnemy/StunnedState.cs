using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class StunnedState : LogicMachineBehaviour<NormalEnemyLogicManager>
{


    public override void OnAwake()
    {
        
    }

    public override void OnEnter()
    {
        DoKnockback(manager.knockbackForce, manager.playerTransform);
    }
    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public async void DoKnockback(float knockbackForce, Transform playerTransform)
    {
        
        Vector3 direction = (transform.position - playerTransform.position).normalized;
        manager.rb.AddForce(direction * knockbackForce, ForceMode.Impulse);

        float time = manager.timeStunned;

        await Task.Delay(TimeSpan.FromSeconds(time));

        if (!active) return;
        manager.enemyHealth.wasAttacked = false;
        logicAnimator.SetBool("isStunned", false);
    }

}
