using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerAttack : MonoBehaviour
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

    private AttackState currentAttackState = AttackState.Attack1;

    private enum AttackState { Attack1, Attack2, Attack3 }





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
        rb = GetComponent<Rigidbody>();

        attackCooldown = GetAttackAnimationDuration(currentAttackState);
        canMove = true;

    }
    void Update()
    {
        Debug.Log(currentAttackState);
        

        if (rb.velocity.magnitude > 0)
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
            canMove = false;

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
            if (attackCooldown <= 0)
            {
                canAttack = true;

            }
        }


        if (rb.velocity.magnitude > attackChainBreakSpeed)
        {
            ResetAttack();
        }
    }

    private void AttackChainCheck()
    {
        if (currentAttackState == AttackState.Attack3)
        {
            return; // No more attacks if already in Attack3
        }

        attackCooldown = GetAttackAnimationDuration(currentAttackState);

        MainAttack();

        currentAttackState++;


        currentChainCooldown = originalChainCooldown;

        CancelInvoke("AllowMove");
        Invoke("AllowMove", attackCooldown);

        attackSound.pitch = GetAttackPitch(currentAttackState);
        attackSound.Play();
    }

    void ResetAttack()
    {

        canAttack = true;
        attackPerformed = false;
        currentAttackState = AttackState.Attack1;
        canMove = true;
        Debug.Log("Move Allowed");
    }

    void AllowMove()
    {
        canMove = true;
        Debug.Log("Move Allowed");
    }


    void MainAttack()
    {
        float damage = currentAttackState == AttackState.Attack3 ? playerDamage + playerDamageFinal : playerDamage;

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
        }

        Debug.Log("Attack Damage: " + damage);
    }

    private float GetAttackAnimationDuration(AttackState attackState)
    {
        switch (attackState)
        {
            case AttackState.Attack1:
                return attackAnimation1Duration;
            case AttackState.Attack2:
                return attackAnimation2Duration;
            case AttackState.Attack3:
                return attackAnimation3Duration;
            default:
                return 0f;
        }
    }

    private float GetAttackPitch(AttackState attackState)
    {
        switch (attackState)
        {
            case AttackState.Attack1:
                return 1.5f;
            case AttackState.Attack2:
                return 1f;
            case AttackState.Attack3:
                return 0.5f;
            default:
                return 1f;
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
