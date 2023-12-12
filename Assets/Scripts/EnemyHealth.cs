using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float colorChangeDuration;
    public float knockBackForce;
    public SpriteRenderer enemySprite;
    public Color32 damageColor;
    public Color32 stunnedColor;
    public Color32 originalColor;
    private Rigidbody rb;
    public bool wasAttacked;
    public bool isStunned;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        originalColor = enemySprite.color;
        damageColor = Color.red;
        stunnedColor = Color.black;
        wasAttacked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        wasAttacked = true;
        StartCoroutine(ChangeColor(damageColor));


        if (currentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }

    public void TakeStunnedDamage(float damage)
    {
        currentHealth -= damage;
        isStunned = true;
        wasAttacked = true;
        StartCoroutine(ChangeColor(stunnedColor));


        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ChangeColor(Color32 Color)
    {
        enemySprite.color = Color;

        yield return new WaitForSeconds(colorChangeDuration);

        enemySprite.color = originalColor;

    }


}
