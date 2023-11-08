using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyPatrol : MonoBehaviour
{
    [Header("References")]
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody rb;
    private Animator animator;
    private Transform currentPoint;

    [Header("Values")]
    public float minDistance;
    public float speed;
    public float waitTime;
    public float knockbackTime;

    [Header("Checks")]
    public bool tookKnockback = false;
    private bool canMove = true;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
       // animator.SetBool("isRunning", true);
        currentPoint = pointB.transform;
        bool canMove = true;
}

    // Update is called once per frame
    void Update()
    {
        
        if(!tookKnockback)
        {
            Vector3 point = currentPoint.position - transform.position;
            if (currentPoint == pointB.transform && canMove)
            {
                rb.velocity = new Vector3(speed, 0, 0);
            }
            else if (currentPoint == pointA.transform && canMove)
            {
                rb.velocity = new Vector3(-speed, 0, 0);
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
        
    }

    private void StopMoving()
    {
        if (currentPoint == pointB.transform)
        {
            canMove = false;

            rb.velocity = new Vector3(0, 0, 0);
            currentPoint = pointA.transform;
            Invoke("ContinueMoving", waitTime);

        }
        else if ( currentPoint == pointA.transform) 
        {
          
            canMove = false;
            rb.velocity = new Vector3(0, 0, 0);
            currentPoint = pointB.transform;

            Invoke("ContinueMoving", waitTime);
        }
    }

    private void ContinueMoving()
    {
        if (currentPoint == pointB.transform)
        {
            canMove = true;
           
            Flip();
            rb.velocity = new Vector3(speed, 0, 0);
        }
        else
        {
            canMove = true;
           
            Flip();
            rb.velocity = new Vector3(-speed, 0, 0);
        }


    }


    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void DoKnockBack(float knockbackForce, Transform playerTransform)
    {
        tookKnockback = true;
        Vector3 direction = (transform.position - playerTransform.position).normalized;
        rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
        Debug.Log(knockbackForce);
        Debug.Log(playerTransform);
        Invoke("ResetKnockback", knockbackTime);
    }

    public void ResetKnockback()
    {
        tookKnockback = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
    }
}
