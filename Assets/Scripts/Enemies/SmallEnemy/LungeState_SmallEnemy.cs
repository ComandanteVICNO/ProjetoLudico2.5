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
    float moveSpeed;

    public Transform playerTransform;
    public Vector3 directionToPlayer;
    public Vector3 lungeDirection;

    public override void OnAwake()
    {
        
    }

    public override void OnEnter()
    {
        logicAnimator.SetBool("canCharge", false);
        manager.spriteAnimator.SetBool("isLunging", true);
        canLunge = true;
        attackPerformed = false;
    }
    public override void OnUpdate()
    {
        GetPlayerTransform();
        if (canLunge)
        {
            LungeAttack();
        }

        if(attackPerformed)
        {
            logicAnimator.SetBool("canLunge", false);
        }
        moveSpeed = manager.rb.velocity.magnitude;
        manager.spriteAnimator.SetFloat("moveSpeed", moveSpeed);
    }

    public override void OnExit()
    {
        manager.spriteAnimator.SetBool("isLunging", false);
    }

    //async void LungeAttack()
    //{
    //    canLunge = false;
    //    //manager.rb.AddForce(manager.directionToPlayer * manager.lungeForce, ForceMode.Impulse);
    //    FindDirectionToPlayer();

    //    manager.rb.AddForce(lungeDirection * manager.lungeForce, ForceMode.Impulse);

    //    var time = manager.lungeDuration;

    //    await Task.Delay(TimeSpan.FromSeconds(time));

    //    if (!active) return;
    //    StopMovement();

    //    attackPerformed = true;
    //}

    async void LungeAttack()
    {
        canLunge = false;
        UseInitialPlayerPosition(); // Use the stored initial player position
        manager.rb.AddForce(lungeDirection * manager.lungeForce, ForceMode.Impulse);

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

    //void FindDirectionToPlayer()
    //{
    //    if (playerTransform == null) return;
    //    else
    //    {
    //        directionToPlayer = playerTransform.position - transform.position;
    //    }

    //    Vector3 direction = playerTransform.position - transform.position;

    //    float dotProduct = Vector3.Dot(direction, transform.right);

    //    if (dotProduct > 0)
    //    {
    //        // Target is on the right
    //        lungeDirection = manager.enemyTransform.right;
    //    }
    //    else if (dotProduct < 0)
    //    {
    //        // Target is on the left
    //        lungeDirection = - manager.enemyTransform.right;
    //    }
    //}

    void UseInitialPlayerPosition()
    {
        if (playerTransform == null) return;

        // Use the stored initial player position for the lunge direction
        directionToPlayer = manager.initialPlayerPosition - transform.position;

        Vector3 direction = manager.initialPlayerPosition - transform.position;

        float dotProduct = Vector3.Dot(direction, transform.right);

        if (dotProduct > 0)
        {
            // Target is on the right
            lungeDirection = manager.enemyTransform.right;
        }
        else if (dotProduct < 0)
        {
            // Target is on the left
            lungeDirection = -manager.enemyTransform.right;
        }
    }

    void GetPlayerTransform()
    {
        if (manager.detectPlayerSphere.playerTransform == null) return;

        playerTransform = manager.detectPlayerSphere.playerTransform;
    }

}
