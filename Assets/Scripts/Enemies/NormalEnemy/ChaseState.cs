using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NormalEnemyPatrol;

public class ChaseState : LogicMachineBehaviour<NormalEnemyLogicManager>
{

    Transform playerTransform;

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
            logicAnimator.SetBool("isStunned", true);
        }


        //if (IsTransformBetweenPoints(manager.playerTransform, manager.pointA, manager.pointB))
        if (CheckOutOfBounds())
        {
            CheckPlayerDirection();
            ChasePlayer();
        }
        else
        {
            manager.rb.velocity = Vector3.zero;
        }
    }

    public override void OnExit()
    {
        logicAnimator.SetBool("canChasePlayer", false);
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

    private bool CheckOutOfBounds()
    {
        if ((Vector2.Distance(manager.transform.position, manager.pointA.transform.position) < manager.minDistanceToPoint) || (Vector2.Distance(manager.transform.position, manager.pointB.transform.position) < manager.minDistanceToPoint))
        {
            return false;
        }
        else { return true; }
    }

    bool IsTransformBetweenPoints(Transform target, Transform point1, Transform point2)
    {
        Vector3 aToB = (point2.position - point1.position).normalized;
        Vector3 aToTarget = (target.position - point1.position).normalized;

        // Check if the dot product is positive, indicating that the target is between the points
        return Vector3.Dot(aToB, aToTarget) > 0 && Vector3.Distance(point1.position, target.position) <= Vector3.Distance(point1.position, point2.position);
    }
}
