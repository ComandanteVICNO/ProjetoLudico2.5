using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform playerCamera;
    private Transform spriteTransform;

    private void Start()
    {
        spriteTransform.GetComponent<Transform>();
        playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        //float cameraRotationY = playerCamera.rotation.eulerAngles.y;
        //Quaternion newRotation = Quaternion.Euler(0, cameraRotationY, 0);

        //// Apply the new rotation to the player
        //spriteTransform.rotation = newRotation;
    }
}
