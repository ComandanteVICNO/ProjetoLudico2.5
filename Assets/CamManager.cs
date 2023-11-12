using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public static CamManager current;

    public CinemachineVirtualCamera focusCamera;

    private void Awake()
    {
        current = this;
    }
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void FocusCamera(Transform follow, Transform lookAt)
    {
        focusCamera.Priority = 11;
        focusCamera.LookAt = lookAt;
        focusCamera.Follow = follow;
    }

    public void UnfocusCamera()
    {
        focusCamera.Priority = 9;
    }
}
