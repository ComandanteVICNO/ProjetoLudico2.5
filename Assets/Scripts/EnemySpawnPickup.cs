using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPickup : MonoBehaviour
{
    public GameObject healthPickup;
    public float launchForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        SpawnPickup();
    }
    void SpawnPickup()
    {
        GameObject spawnedPickup = Instantiate(healthPickup, transform.position, Quaternion.identity);

        Rigidbody pickupRb = spawnedPickup.GetComponent<Rigidbody>();
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);

        Vector2 randomDirection = new Vector2(randomX, randomY).normalized;

        pickupRb.velocity = randomDirection * launchForce;
    }

    
}
