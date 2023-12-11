using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScritp : MonoBehaviour
{
    PlayerHealth playerHealth;
    public float damage;
    public GameObject projectile;


    private void OnTriggerEnter(Collider other)
    {
        playerHealth = other.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        Destroy(projectile);
    }



}
