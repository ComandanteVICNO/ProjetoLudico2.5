using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaseState_NormalEnemy : LogicMachineBehaviour<NormalEnemyLogicManager>
{

    Transform playerTransform;
    float currentStepCooldown;
    public enum PlayerDir
    {
        Left,
        Right,
    }

    private PlayerDir playerDir;


    public override void OnAwake()
    {

    }

    public override void OnEnter()
    {
        manager.animator.SetBool("isChasing", true);
        currentStepCooldown = manager.chaseStepSpeed;
        playerTransform = manager.chaseHitbox.playerTransform;
    }
    public override void OnUpdate()
    {
        if(manager.attackHitbox.isPlayerDetected)
        {
            logicAnimator.SetBool("canAttackPlayer", true);
        }
        if (manager.chaseHitbox.isPlayerDetected)
        {
            logicAnimator.SetBool("canChasePlayer", true);
        }
        if (!manager.chaseHitbox.isPlayerDetected)
        {
            logicAnimator.SetBool("canChasePlayer", false);
        }
        if (manager.enemyHealth.wasAttacked)
        {
            logicAnimator.SetBool("wasAttacked", true);
        }

        CheckPlayerDirection();
        ChasePlayer();
        ChaseFootSteps();



    }

    public override void OnExit()
    {
        logicAnimator.SetBool("canChasePlayer", false);
        manager.animator.SetBool("isChasing", false);
    }

    private void CheckPlayerDirection()
    {

        if (playerTransform == null) return;

        else
        {
            if (transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                playerDir = PlayerDir.Left;

            }
            else if (transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                playerDir = PlayerDir.Right;
            }
        }
    }

    private void ChasePlayer()
    {

        if (playerDir == PlayerDir.Left)
        {
            manager.rb.velocity = new Vector3(-manager.chaseSpeed, 0, 0);
        }
        else if (playerDir == PlayerDir.Right)
        {
            manager.rb.velocity = new Vector3(manager.chaseSpeed, 0, 0);
        }

    }
    public void ChaseFootSteps()
    {

        currentStepCooldown -= Time.deltaTime;
        if (currentStepCooldown < 0)
        {

            int randomFootstepSound = UnityEngine.Random.Range(0, manager.audioSteps.Length);
            manager.footstepAudioSource.PlayOneShot(manager.audioSteps[randomFootstepSound]);
            currentStepCooldown = manager.patrolStepSpeed;
        }
        else return;

    }
}
