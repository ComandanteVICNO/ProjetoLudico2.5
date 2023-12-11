using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
    public float playerStunningDamage;
    public float playerDamageFinal;
    public float knockBackAmount;

    [Header("Attack and Animation Cooldown")]
    public float attackCooldown;
    public float attackChainBreakSpeed;


    [Header("Attack Checks")]
    public bool attackPerformed = false;
    public bool canAttack = true;
    public bool canMove = true;
    public LayerMask enemyLayers;

    [Header("Animation Clip References")]
    public AnimationClip normalAttackAnimation;
    public AnimationClip stunAttackAnimation;

    [Header("Animation Clip Durations")]
    public float normalAttackAnimationTime;
    public float stunAttackAnimationTime;

    public Coroutine normalAttackCoroutine;
    public Coroutine stunAttackCoroutine;

    private void Start()
    {
        canMove = true;   
        rb = GetComponent<Rigidbody>();
        normalAttackAnimationTime = normalAttackAnimation.length;
        stunAttackAnimationTime = stunAttackAnimation.length;
    }


    void Update()
    {

        //Verify attack input
        if (UserInput.instance.controls.Player.MainAttack.WasPressedThisFrame() && movController.isGrounded && canAttack)
        {
            if (normalAttackCoroutine == null)
            {
                normalAttackCoroutine = StartCoroutine(DoMainAttack());
            }
            
        }

        if(UserInput.instance.controls.Player.StunAttack.WasPerformedThisFrame() && movController.isGrounded && canAttack)
        {
            if(stunAttackCoroutine == null)
            {
                stunAttackCoroutine = StartCoroutine(DoStunAttack());
            }

        }
    }


    public IEnumerator DoMainAttack()
    {
        MainAttack();
        animator.SetBool("NormalAttack", true);
        attackPerformed = true;
        canAttack = false;
        canMove = false;

        yield return new WaitForSeconds(normalAttackAnimationTime);

        attackPerformed = false;
        canAttack = true;
        canMove = true;
        animator.SetBool("NormalAttack", false);
        normalAttackCoroutine = null;
    }

    public IEnumerator DoStunAttack()
    {
        StunnedAttack();
        animator.SetBool("StunAttack", true);
        attackPerformed = true;
        canAttack = false;
        canMove = false;

        yield return new WaitForSeconds(stunAttackAnimationTime);

        attackPerformed = false;
        canAttack = true;
        canMove = true;
        animator.SetBool("StunAttack", false);
        stunAttackCoroutine = null;
    }


    void MainAttack()
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

    public void StunnedAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        Debug.Log("Did stun attack");
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.GetComponent<EnemyHealth>() != null)
            {
                Debug.Log("enemy was stunned");
                enemy.GetComponent<EnemyHealth>().TakeStunnedDamage(playerStunningDamage);
            }

            if (enemy.GetComponent<Fracture>() != null)
            {
                enemy.GetComponent<Fracture>().BreakObject();
                Debug.Log("break");
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }
}
