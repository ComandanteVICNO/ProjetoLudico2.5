using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetectPlayer : MonoBehaviour
{
    public bool isPlayerDetected;
    public Transform playerTransform;


    private void Start()
    {
        isPlayerDetected = false;
    }

    private void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = true;
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = false;
            playerTransform = null;
        }
    }

    public bool PlayerDectionStatus()
    {
        return isPlayerDetected;
    }
}
