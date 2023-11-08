using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCones : MonoBehaviour
{
    public GameObject coneObject;
    public Transform conePosition;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 100; i++)
        {
            Instantiate(coneObject, conePosition);
        }
    }
}
