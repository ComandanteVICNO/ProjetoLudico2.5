using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPickup : MonoBehaviour
{
    public GameObject healthPickup;
    public GameObject energyPickup;
    public float launchForce;
    [Header("Spawn Chance (0% - 100%)")]
    public float healthSpawnChance;
    public float energySpawnChance;
    bool wasObjectSpawned = false;
    public string chosenEnergy = "Energy";
    public string chosenHealth = "Health";
    string objectToSpawn;
    SmallEnemyHealth smallEnemyHealth;
    NormalEnemyHealth normalEnemyHealth;

    


    // Start is called before the first frame update
    void Start()
    {
        smallEnemyHealth = GetComponent<SmallEnemyHealth>();
        normalEnemyHealth = GetComponent<NormalEnemyHealth>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (normalEnemyHealth != null)
        {
            if (normalEnemyHealth.currentHealth <= 0)
            {
                SelectPickup();
                wasObjectSpawned = true;
            }
        }
        else if (smallEnemyHealth != null)
        {
            if (smallEnemyHealth.currentHealth <= 0)
            {
                SelectPickup();
                wasObjectSpawned = true;
            }
        }
    }

    public void SelectPickup()
    {
        float randomNumber = Random.Range(0, 100);
        if (randomNumber < 50)
        {
            objectToSpawn = chosenEnergy;
        }
        else if(randomNumber >= 50)
        {
            objectToSpawn = chosenHealth;
        }
        Debug.Log(objectToSpawn);
        SpawnPickup();
    }

    void SpawnPickup()
    {
        if (wasObjectSpawned) return;
        float randomNumber = Random.Range(0, 100);
        
        if(objectToSpawn == chosenEnergy) 
        {
            if (randomNumber > energySpawnChance) return;
            GameObject spawnedPickup = Instantiate(energyPickup, transform.position, Quaternion.identity);

            Rigidbody pickupRb = spawnedPickup.GetComponent<Rigidbody>();
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);

            Vector2 randomDirection = new Vector2(randomX, randomY).normalized;

            pickupRb.velocity = randomDirection * launchForce;
        }
        else if(objectToSpawn == chosenHealth)
        {
            if (randomNumber > healthSpawnChance) return;
            GameObject spawnedPickup = Instantiate(healthPickup, transform.position, Quaternion.identity);

            Rigidbody pickupRb = spawnedPickup.GetComponent<Rigidbody>();
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);

            Vector2 randomDirection = new Vector2(randomX, randomY).normalized;

            pickupRb.velocity = randomDirection * launchForce;
        }

        

        
    }

    
}
