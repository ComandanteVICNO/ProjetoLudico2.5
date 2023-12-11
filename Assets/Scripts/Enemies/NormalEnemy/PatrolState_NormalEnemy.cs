using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState_NormalEnemy : LogicMachineBehaviour<NormalEnemyLogicManager>
{
    bool canMove;
    bool waiting;
    Transform currentPoint;
    public Vector2 waitTime;

    public override void OnAwake()
    {
        canMove = true;
        waiting = false;
        waitTime = new Vector2(manager.minWaitTime, manager.maxWaitTime);
    }

    public override void OnEnter()
    {
        currentPoint = manager.pointA.transform;
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

    }

    public override void OnExit()
    {
        
    }
        

    //void Patrol()
    //{
    //    if (currentPoint == manager.pointB.transform && !waiting)
    //    {
    //        manager.rb.velocity = new Vector3(manager.patrolSpeed, 0, 0);
    //        manager.enemyTransform.localScale = new Vector3(-1, 1, 1);
    //    }
    //    else if (currentPoint == manager.pointA.transform && !waiting)
    //    {
    //        manager.rb.velocity = new Vector3(-manager.patrolSpeed, 0, 0);
    //        manager.enemyTransform.localScale = new Vector3(1, 1, 1);
    //    }

    //    if (Vector2.Distance(transform.position, currentPoint.position) < manager.minDistanceToPoint)
    //    {
    //        WaitForNewPoint();
    //    }
    //}
    
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
}
