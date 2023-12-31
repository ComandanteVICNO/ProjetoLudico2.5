using DG.Tweening;
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
    public RectTransform playerHealthBar;
    public RectTransform playerHealthBackgroundBar;
    public float barBackgroundWaitTime;
    public float barBackgroundAnimationTime;
    float currentBarValue;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip hurtSound;


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
        UpdateHealth();

        
    }

    public void TakeDamage(float damage)
    {
        if (DeathHandler.instance.isDead) return;
        if(!DeathHandler.instance.canBeAttacked) return;
        currentHealth -= damage;
        StartCoroutine(UpdateHealth());
        StartCoroutine(ChangeColor());

        audioSource.PlayOneShot(hurtSound);

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


    public IEnumerator UpdateHealth()
    {
        currentBarValue = (currentHealth * 1) / maxHealth;

        playerHealthBar.localScale = new Vector3(currentBarValue, 1, 1);
        
        yield return new WaitForSeconds(barBackgroundWaitTime);

        playerHealthBackgroundBar.DOScale(new Vector3(currentBarValue, 1, 1), barBackgroundAnimationTime).SetEase(Ease.Linear);
    }

    public void HealthPickup(float health)
    {
        currentHealth += health;
        StartCoroutine(UpdateHealth());
        
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

   
}
