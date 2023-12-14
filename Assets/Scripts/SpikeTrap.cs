using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Transform checkPoint;
    public Transform playerTrasform;
    public PlayerMovController movController;
    public PlayerHealth playerHealth;
    public Rigidbody rb;
    public float waitTime;
    public Coroutine teleportCoroutine;
    

    public void CancelMovement()
    {
        movController.canMove = false;
        movController.canJump = false;
        
        rb.velocity = Vector3.zero;
    }

    public void AllowMovement()
    {
        movController.canMove = true;
        movController.canJump = true;
        
    }

    public void MovePlayer()
    {
        playerTrasform.position = checkPoint.position;
    }

    IEnumerator Teleport()
    {
        CancelMovement();
        MovePlayer();

        yield return new WaitForSecondsRealtime(waitTime);

        AllowMovement();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        {
            rb = other.GetComponent<Rigidbody>();
            playerTrasform = other.transform;
            playerHealth = other.GetComponent<PlayerHealth>();
            movController = other.GetComponent<PlayerMovController>();
            if(teleportCoroutine != null)
            {
                StopCoroutine(teleportCoroutine);
            }
            teleportCoroutine = StartCoroutine(Teleport());
        }
    }

}
