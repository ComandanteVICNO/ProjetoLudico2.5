using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Camera playerCamera;
    public float cameraDistance;
    public float cameraSpeed = 5;

    private void Update()
    {
        Vector3 playerPos = player.transform.position;

        var newpos = new Vector3(playerPos.x, playerPos.y, playerPos.z - cameraDistance);
        playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, newpos, Time.deltaTime * cameraSpeed);
    }

}
