using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalEnemyHealth : MonoBehaviour
{
    public GameObject enemyObject;
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

    [Header("Death Stuff")]
    public Animator stateAnimator;
    public Animator spriteAnimator;
    public AnimationClip deathClip;
    public GameObject attackPoint;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip hurtSound;


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
        audioSource.PlayOneShot(hurtSound);

        if (currentHealth <= 0) 
        {
            StartCoroutine(Death());
        }
    }

    public void TakeStunnedDamage(float damage)
    {
        currentHealth -= damage;
        isStunned = true;
        wasAttacked = true;
        StartCoroutine(ChangeColor(stunnedColor));
        audioSource.PlayOneShot(hurtSound);

        if (currentHealth <= 0)
        {
            StartCoroutine(Death());  
        }
    }

    IEnumerator Death()
    {

        spriteAnimator.SetBool("isAttacking", false);
        spriteAnimator.SetBool("isPatroling", false);
        spriteAnimator.SetBool("isChasing", false);
        spriteAnimator.SetBool("isDead", true);

        stateAnimator.SetBool("canChase", false);
        stateAnimator.SetBool("canAttack", false) ;
        stateAnimator.SetBool("wasAttacked", false);
        stateAnimator.SetBool("isDead", true) ;


        yield return new WaitForSecondsRealtime(deathClip.length);

        Destroy(enemyObject);

    }

    IEnumerator ChangeColor(Color32 Color)
    {
        enemySprite.color = Color;

        yield return new WaitForSeconds(colorChangeDuration);

        enemySprite.color = originalColor;

    }


}
