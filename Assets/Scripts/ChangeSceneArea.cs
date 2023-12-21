using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneArea : MonoBehaviour
{
    public Transform playerTrasform;
    public PlayerMovController movController;
    public Rigidbody rb;
    public float waitTime;

    //temporary cause it's still no changing scenes
    public Transform teleportLocation;



    // Update is called once per frame
    void Update()
    {
        
    }

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
        playerTrasform.position = teleportLocation.position;
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
            movController = other.GetComponent<PlayerMovController>();
            StartCoroutine(Teleport());
        }
    }
}
