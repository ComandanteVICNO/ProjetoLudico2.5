using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoors : MonoBehaviour
{
    public GameObject[] doors;
    public GameObject[] enemies;

    public bool doorsClosed;
    public bool wasEncounterFinished;
    public Transform otherTransform;
    private void Start()
    {
        doorsClosed = false;
        wasEncounterFinished = false;
    }


    void Update()
    {
        if (!doorsClosed) return;
        else
        {
            if (AllEnemiesDestroyed())
            {
                DoOpenDoors();
                wasEncounterFinished = true;
            }
        }
    }


    bool AllEnemiesDestroyed()
    {
        foreach (GameObject enemy in enemies) 
        {
            if(enemy != null)
            {
                return false;
            }
        }

        return true;
    }

    public void DoOpenDoors()
    {
        foreach (GameObject doorPrefab in doors)
        {
            DoorAnimation doorAnimation = doorPrefab.GetComponentInChildren<DoorAnimation>();

            doorAnimation.OpenDoors();
        }

        doorsClosed = false;
    }

    public void DoCloseDoors()
    {
        foreach(GameObject doorPrefab in doors) 
        {
            DoorAnimation doorAnimation = doorPrefab.GetComponentInChildren<DoorAnimation>();

            doorAnimation.CloseDoors();
        }
        
        doorsClosed = true;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(wasEncounterFinished) return;
            else
            {
                DoCloseDoors();
            }

            
        }
    }

}
