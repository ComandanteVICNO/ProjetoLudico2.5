using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float colorChangeDuration;
    public SpriteRenderer playerSprite;
    public Color32 damageColor;
    public Color32 originalColor;


    void Start()
    {
        currentHealth = maxHealth;
        originalColor = playerSprite.color;
        damageColor = Color.red;

    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        StartCoroutine(ChangeColor());

        if (currentHealth <= 0) 
        {
            //Destroy(gameObject);
            Debug.Log("Player Dead");
        }
    }

    IEnumerator ChangeColor()
    {
        playerSprite.color = damageColor;

        yield return new WaitForSeconds(colorChangeDuration);

        playerSprite.color = originalColor;

    }

}
