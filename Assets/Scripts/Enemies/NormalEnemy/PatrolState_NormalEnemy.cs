using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState_NormalEnemy : LogicMachineBehaviour<NormalEnemyLogicManager>
{
    bool canMove;
    bool waiting;
    Transform currentPoint;
    public Vector2 waitTime;
    private CancellationTokenSource footStepsCancellationTokenSource;
    float currentStepCooldown;
    public override void OnAwake()
    {
        canMove = true;
        waiting = false;
        waitTime = new Vector2(manager.minWaitTime, manager.maxWaitTime);
    }

    public override void OnEnter()
    {
        currentStepCooldown = manager.patrolStepSpeed;
        footStepsCancellationTokenSource = new CancellationTokenSource();
        currentPoint = manager.pointA.transform;
        manager.animator.SetBool("isPatroling", true);
    }

    public override void OnUpdate()
    {
        if (manager.enemyHealth.wasAttacked)
        {
            logicAnimator.SetBool("wasAttacked", true);
        }

        if (manager.chaseHitbox.isPlayerDetected)
        {
            logicAnimator.SetBool("canChasePlayer", true);
        }
        else
        {
            MoveToTargetPoint();

        }

        if(manager.rb.velocity.magnitude > 1f)
        {
            PatrolFootSteps();
            
        }

        manager.animator.SetFloat("moveSpeed", manager.rb.velocity.magnitude);
    }

    public override void OnExit()
    {
        footStepsCancellationTokenSource?.Cancel();
        footStepsCancellationTokenSource?.Dispose();
        manager.animator.SetBool("isPatroling", false);
    }
        

    
    
    async void WaitForNewPoint()
    {
        waiting = true;
        if(waiting)
        {
            manager.rb.velocity = new Vector3(0,0,0);
            
        }

        if (currentPoint == manager.pointA.transform)
        {
            currentPoint = manager.pointB.transform;
        }
        else if (currentPoint == manager.pointB.transform)
        {
            currentPoint = manager.pointA.transform;
        }

        var time = UnityEngine.Random.Range(waitTime.x, waitTime.y);

        await Task.Delay(TimeSpan.FromSeconds(time));

        if (!active) return;

        
        waiting = false;
    }
    void MoveToTargetPoint()
    {
        if (!waiting)
        {
            

            Vector3 direction = currentPoint.position - transform.position;

            float dotProduct = Vector3.Dot(direction, transform.right);

            if (dotProduct > 0)
            {
                // Target is on the right
                manager.enemyTransform.localScale = new Vector3(-1, 1, 1);
            }
            else if (dotProduct < 0)
            {
                // Target is on the left
                manager.enemyTransform.localScale = new Vector3(1, 1, 1);
            }


            direction.Normalize();

            manager.rb.velocity = direction * manager.patrolSpeed;




        }
        if (Vector2.Distance(transform.position, currentPoint.position) < manager.minDistanceToPoint)
        {
            WaitForNewPoint();
        }
    }
    public void PatrolFootSteps()
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
