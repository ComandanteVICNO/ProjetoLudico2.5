using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{

    public GameObject fractured;
    public float breakForce;
    public Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            BreakObject();
        }
    }

    public void BreakObject()
    {
        GameObject fract =  Instantiate(fractured, transform.position, transform.rotation);

        Rigidbody[] childRigidbodies = fract.GetComponentsInChildren<Rigidbody>();
        Vector3 playerPosition = playerTransform.position;


        foreach (Rigidbody rb in childRigidbodies)
        {

            Vector3 forceDirection = (rb.transform.position - playerPosition).normalized;
            forceDirection.z = 0f;
            rb.AddForce(forceDirection * breakForce, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }

    
}
