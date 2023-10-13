using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float colorChangeDuration;
    private float currentColorChangeDuration;
    public SpriteRenderer enemySprite;
    public Color32 damageColor;
    public Color32 originalColor;
    public bool enemyDamaged = false;

    public GameObject bloodParticles;
    public Transform bloodParticlesLocation;

    void Start()
    {
        currentColorChangeDuration = colorChangeDuration;
        currentHealth = maxHealth;
        originalColor = enemySprite.color;
        damageColor = Color.red;
        enemyDamaged = false;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        enemyDamaged = true;

        Instantiate(bloodParticles, bloodParticlesLocation.position, bloodParticlesLocation.rotation);

        if (currentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }

    private void ChangeColor()
    {
        
        if (enemyDamaged)
        {
            enemySprite.color = damageColor;
            currentColorChangeDuration -= Time.deltaTime;
            if (currentColorChangeDuration <= 0)
            {
                enemySprite.color = originalColor;
                enemyDamaged = false;
                currentColorChangeDuration = colorChangeDuration;
            }
        }
    }


}
