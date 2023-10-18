using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInSight : MonoBehaviour
{
    public bool isPlayerInSight = false;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInSight=true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInSight = false;
        }
    }
}
