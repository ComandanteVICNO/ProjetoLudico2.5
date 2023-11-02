using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public PlayerMovController movController;
    public Transform attackPoint;
    public AudioSource attackSound;
    public Rigidbody rb;

    [Header("Player Damage & Range")]
    public float attackRange = 0.5f;
    public float playerDamage;
    public float playerDamageFinal;

    [Header("Attack and Animation Cooldown")]
    public float originalChainCooldown;
    public float currentChainCooldown;
    public float attackCooldown;
    public float attackChainBreakSpeed;
    
    public bool attack1 = false;
    public bool attack2 = false;
    public bool attack3 = false;


    [Header("Attack Checks")]
    public bool attackPerformed = false;
    public bool canAttack = true;
    public bool canMove = true;
    public LayerMask enemyLayers;

    [Header("Animation Clip References")]
    public AnimationClip attackAnimation1;
    public AnimationClip attackAnimation2;
    public AnimationClip attackAnimation3;

    [Header("Animation Clip Durations")]
    public float attackAnimation1Duration;
    public float attackAnimation2Duration;
    public float attackAnimation3Duration;

    private void Start()
    {
        currentChainCooldown = originalChainCooldown;

        canMove = true;
        
        rb = GetComponent<Rigidbody>();

        attackAnimation1Duration = attackAnimation1.length;
        attackAnimation2Duration = attackAnimation2.length;
        attackAnimation3Duration = attackAnimation3.length;
        
    }


    void Update()
    {
       

        if (rb.velocity.magnitude > 0.5f)
        {
            animator.SetFloat("ResetAttack", 0.1f);
        }
        else
        {
            animator.SetFloat("ResetAttack", Mathf.Abs(currentChainCooldown));
        }

        //Verify attack input
        if (UserInput.instance.controls.Player.MainAttack.WasPressedThisFrame() && movController.isGrounded && canAttack)
        {
            AttackChainCheck();
            attackPerformed = true;
            canAttack = false;
            canMove=false;

        }

        //Perform Chain reset countdown
        if (attackPerformed)
        {
            currentChainCooldown -= Time.deltaTime;
            if (currentChainCooldown <= 0)
            {
                //if countdown = 0, reset attack values
                ResetAttack();
            }
        }

        if (!canAttack)
        {
            
            attackCooldown -= Time.deltaTime;
            if(attackCooldown <= 0)
            {
                canAttack = true;
                
            }
        }
        
        if(rb.velocity.magnitude > attackChainBreakSpeed)
        {
            attack1 = false;
            attack2 = false;
            attack3 = false;
        }
    }

    private void AttackChainCheck()
    {
        
        //verify which attack chain is current 
        if (!attack1 && !attack2 && !attack3)
        {
            
            attackCooldown = attackAnimation1Duration;

            attack1 = true;

            MainAttack();
            animator.SetTrigger("Attack1");

            CancelInvoke("AllowMove");
            Invoke("AllowMove", attackCooldown);


            attackSound.pitch = 1.5f;
            attackSound.Play();

            currentChainCooldown = originalChainCooldown;
        }

        else if (attack1 && !attack2 && !attack3)
        {
            
            attackCooldown = attackAnimation2Duration;

            attack1 = true;
            attack2 = true;

            CancelInvoke("AllowMove");
            Invoke("AllowMove", attackCooldown);

            MainAttack();
            animator.SetTrigger("Attack2");

            attackSound.pitch = 1f;
            attackSound.Play();

            currentChainCooldown = originalChainCooldown;
        }

        else if(attack1 && attack2 && !attack3)
        {
            attackCooldown = attackAnimation3Duration;

            attack1 = true;
            attack2 = true;
            attack3 = true;

            CancelInvoke("AllowMove");
            Invoke("AllowMove", attackCooldown);

            MainAttack();
            animator.SetTrigger("Attack3");

            attackSound.pitch = 0.5f;
            attackSound.Play();
            
            currentChainCooldown = originalChainCooldown;
        }
        else
        {

        }

    }

    void ResetAttack()
    {
        canAttack = true;
        attackPerformed = false;
        currentChainCooldown = originalChainCooldown;
        
        attack1 = false;
        attack2 = false;
        attack3 = false;
        
    }

    void AllowMove()
    {
        canMove = true;
        
    }


    void MainAttack()
    {
        if (!attack3)
        {
            //animator.SetTrigger("Attack");

            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.GetComponent<EnemyHealth>() != null)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(playerDamage);
                }

                if (enemy.GetComponent<Fracture>() != null)
                {
                    enemy.GetComponent<Fracture>().BreakObject();
                    Debug.Log("break");
                }

            }
            
        }
        else 
        {
            //animator.SetTrigger("Attack");

            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(playerDamage + playerDamageFinal);
                
            }
        }
        

    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }
}
