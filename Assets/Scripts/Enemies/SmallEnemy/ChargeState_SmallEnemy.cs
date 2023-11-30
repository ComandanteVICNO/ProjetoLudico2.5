using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ChargeState_SmallEnemy : LogicMachineBehaviour<SmallEnemyLogicManager>
{
    float originalCharge;
    float currentCharge;
    bool canCharge;
    private CancellationTokenSource cancellationTokenSource;

    public override void OnAwake()
    {
        cancellationTokenSource = new CancellationTokenSource();
    }

    public override void OnEnter()
    {
        logicAnimator.SetBool("canLunge", false);
        canCharge = true;
        originalCharge = manager.lungeChargeTime;
        currentCharge = originalCharge;
        cancellationTokenSource = new CancellationTokenSource();
    }
    public override void OnUpdate()
    {
        if (manager.enemyHealth.wasAttacked)
        {
            logicAnimator.SetBool("wasAttacked", true);
        }

        ChargeLunge();

        StopMovement();
    }

    public override void OnExit()
    {
        logicAnimator.SetBool("canCharge", false);
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
        canCharge = false;
    }

    void StopMovement()
    {
        manager.rb.velocity = Vector3.zero;
    }

    async void ChargeLunge()
    {

        var time = manager.lungeChargeTime;
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(time), cancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            
            return;
        }
        if (!active) return;

        logicAnimator.SetBool("canLunge", true);
        


    }

}
