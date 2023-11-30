using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Threading.Tasks;
using UnityEngine;

public class LungeState_SmallEnemy : LogicMachineBehaviour<SmallEnemyLogicManager>
{
    bool canLunge;
    bool attackPerformed;
    public override void OnAwake()
    {
        
    }

    public override void OnEnter()
    {
        logicAnimator.SetBool("canCharge", false);
        canLunge = true;
        attackPerformed = false;
    }
    public override void OnUpdate()
    {
        if (canLunge)
        {
            LungeAttack();
        }

        if(attackPerformed)
        {
            logicAnimator.SetBool("canLunge", false);
        }
    }

    public override void OnExit()
    {
        
    }

    async void LungeAttack()
    {
        canLunge = false;
        manager.rb.AddForce(manager.directionToPlayer * manager.lungeForce, ForceMode.Impulse);

        var time = manager.lungeDuration;
        await Task.Delay(TimeSpan.FromSeconds(time));
        if (!active) return;
        StopMovement();
        attackPerformed = true;
    }

    void StopMovement()
    {
        manager.rb.velocity = Vector3.zero; 
    }

}
