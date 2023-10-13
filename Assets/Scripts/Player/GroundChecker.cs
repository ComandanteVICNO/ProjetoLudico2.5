using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public PlayerMovController movController;

    public float coyoteTime = 0.25f;
    public bool isCoyote;

    private void OnTriggerEnter(Collider other)
    {
        movController.isGrounded = true;
        isCoyote = false;
    }

    private void OnTriggerStay(Collider other)
    {
        movController.isGrounded = true;
        isCoyote = false;
    }



    private void OnTriggerExit(Collider other)
    {
        isCoyote = true;
        movController.isGrounded = false;
        Invoke("doCoyoteTime", coyoteTime);
    }

    private void doCoyoteTime()
    {
        
        isCoyote = false;
    }
}
