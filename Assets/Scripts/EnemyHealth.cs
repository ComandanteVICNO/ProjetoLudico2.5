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
    public Color32 originalColor;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        originalColor = enemySprite.color;
        damageColor = Color.red;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        StartCoroutine(ChangeColor());
        Knockback();


        if (currentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ChangeColor()
    {
        enemySprite.color = damageColor;

        yield return new WaitForSeconds(colorChangeDuration);

        enemySprite.color = originalColor;

    }

    private void Knockback()
    {
        Vector3 oppositeDirection = transform.forward;

        rb.AddForce(oppositeDirection * knockBackForce, ForceMode.Impulse);
    }

}
