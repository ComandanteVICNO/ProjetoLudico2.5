using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class NormalEnemyPatrol : MonoBehaviour
{
    [Header("References")]
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody rb;
    private Animator animator;
    private Transform currentPoint;
    private Transform currentPosition;
    public DetectPlayer boxDetectPlayer;
    public DetectPlayer sphereDetectPlayer;
    private Transform playerTransform;


    public enum PlayerDir
    {
        Left,
        Right,
    }

    private PlayerDir playerDir;

    [Header("Values")]
    public float minDistance;
    public float speed;
    public float waitTime;
    [Header("Bool Checks")]
    public bool canChasePlayer;
    public bool wasAttacked;
    private bool canMove = true;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
        // animator.SetBool("isRunning", true);
        currentPoint = pointA.transform;
        currentPosition = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        FreezeZAxix();
        CheckOutOfBounds();

        if (!boxDetectPlayer.PlayerDectionStatus())
        {
            Patrol();
            
        }
        else
        {
            CheckPlayerDirection();
            
            if(canChasePlayer)
            {
                ChasePlayer();
            }
            else
            {
                Patrol();
            }
        }
    }

    #region Patrolling
    private void Patrol()
    {
        
        if (currentPoint == pointB.transform && canMove)
        {
            rb.velocity = new Vector3(speed, 0, 0);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (currentPoint == pointA.transform && canMove)
        {
            rb.velocity = new Vector3(-speed, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < minDistance && currentPoint == pointB.transform)
        {

            StopMoving();
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < minDistance && currentPoint == pointA.transform)
        {
            StopMoving();
        }
    }

    private void StopMoving()
    {
        if (currentPoint == pointB.transform)
        {
            canMove = false;
            
            rb.velocity = new Vector3(0, 0, 0);
            currentPoint = pointA.transform;
            Invoke("MoveToNextPoint", waitTime);

        }
        else if (currentPoint == pointA.transform)
        {
            
            canMove = false;
            rb.velocity = new Vector3(0, 0, 0);
            currentPoint = pointB.transform;

            Invoke("MoveToNextPoint", waitTime);
        }
    }

    private void MoveToNextPoint()
    {
        if (currentPoint == pointB.transform)
        {
            canMove = true;
            
            
            rb.velocity = new Vector3(speed, 0, 0);
        }
        else
        {
            canMove = true;
            
            
            rb.velocity = new Vector3(-speed, 0, 0);
        }


    }
    #endregion

    private void CheckPlayerDirection()
    {
        Transform playerPos = boxDetectPlayer.playerTransform;

        if (playerPos == null) return;

        else
        {
            if(transform.position.x > playerPos.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                playerDir = PlayerDir.Left;

            }
            else if(transform.position.x < playerPos.position.x)
            {
                transform.localScale = new Vector3(-1,1,1);
                playerDir = PlayerDir.Right;
            }
        }


    }

    private void ChasePlayer()
    {
        if(boxDetectPlayer.playerTransform != null)
        {
            if (sphereDetectPlayer.PlayerDectionStatus())
            {
                rb.velocity = new Vector3(0, 0, 0);
            }
            else if(playerDir == PlayerDir.Left)
            {
                rb.velocity = new Vector3(-speed, 0, 0);
            }
            else if(playerDir == PlayerDir.Right)
            {
                rb.velocity = new Vector3(speed, 0, 0);
            }
        }

    }
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }


    private void CheckOutOfBounds()
    {
        if ((Vector2.Distance(transform.position, pointA.transform.position) < minDistance) || (Vector2.Distance(transform.position, pointB.transform.position) < minDistance))
        {
            canChasePlayer = false;
        }
        else { canChasePlayer = true; }
    }
    
    private void FreezeZAxix()
    {
        if(transform.localPosition.z < 0)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);

        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);

    }
}
