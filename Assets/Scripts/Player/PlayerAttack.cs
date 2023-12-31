using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack current;

    [Header("References")]
    public Animator animator;
    public PlayerMovController movController;
    public Transform attackPoint;
    public AudioSource attackSound;
    public Rigidbody rb;

    [Header("Player Damage & Range")]
    public float attackRange = 0.5f;
    public float playerDamage;
    public float maxEnergy;
    public float currentEnergy;
    public float playerStunningDamage;
    public float playerDamageFinal;
    public float knockBackAmount;

    [Header("Attack and Animation Cooldown")]
    public float attackCooldown;
    public float attackChainBreakSpeed;


    [Header("Attack Checks")]
    public bool isAttacking = false;
    public bool canAttack = true;
    public bool canMove = true;
    public bool isAttackAllow = true;
    public LayerMask enemyLayers;

    [Header("Animation Clip References")]
    public AnimationClip normalAttackAnimation;
    public AnimationClip stunAttackAnimation;

    [Header("Animation Clip Durations")]
    public float normalAttackAnimationTime;
    public float stunAttackAnimationTime;

    [Header("Energia UI")]
    public RectTransform playerEnergyBar;
    public RectTransform playerEnergyBackgroundBar;
    public float barBackgroundWaitTime;
    public float barBackgroundAnimationTime;
    float currentBarValue;


    public Coroutine normalAttackCoroutine;
    public Coroutine stunAttackCoroutine;

    private void Start()
    {
        current = this;
        canMove = true;
        currentEnergy = maxEnergy;
        rb = GetComponent<Rigidbody>();
        normalAttackAnimationTime = normalAttackAnimation.length;
        stunAttackAnimationTime = stunAttackAnimation.length;

    }


    void Update()
    {

        //Verify attack input
        if (UserInput.instance.controls.Player.MainAttack.WasPressedThisFrame() && movController.isGrounded && canAttack)
        {
            if (!isAttackAllow) return;
            if (normalAttackCoroutine == null)
            {
                normalAttackCoroutine = StartCoroutine(DoMainAttack());
            }
            
        }

        if(UserInput.instance.controls.Player.StunAttack.WasPerformedThisFrame() && movController.isGrounded && canAttack)
        {
            if (!isAttackAllow) return;
            float energyNeed = maxEnergy / 3;
            if (currentEnergy <= energyNeed) return;
            else
            {
                if (stunAttackCoroutine == null)
                {
                    stunAttackCoroutine = StartCoroutine(DoStunAttack());
                    currentEnergy -= energyNeed;
                    StartCoroutine(UpdateEnergyUI());

                }
            }
        }
    }


    public IEnumerator DoMainAttack()
    {
        MainAttack();
        animator.SetBool("NormalAttack", true);
        isAttacking = true;
        canAttack = false;
        canMove = false;

        yield return new WaitForSeconds(normalAttackAnimationTime);

        isAttacking = false;
        canAttack = true;
        canMove = true;
        animator.SetBool("NormalAttack", false);
        normalAttackCoroutine = null;
    }

    public IEnumerator DoStunAttack()
    {
        StunnedAttack();
        
        animator.SetBool("StunAttack", true);
        isAttacking = true;
        canAttack = false;
        canMove = false;

        yield return new WaitForSeconds(stunAttackAnimationTime);

        isAttacking = false;
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
            if (enemy.GetComponent<SmallEnemyHealth>() != null)
            {
                enemy.GetComponent<SmallEnemyHealth>().TakeDamage(playerDamage);
            }

            if (enemy.GetComponent<NormalEnemyHealth>() != null)
            {
                enemy.GetComponent<NormalEnemyHealth>().TakeDamage(playerDamage);
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
        
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.GetComponent<SmallEnemyHealth>() != null)
            {
                
                enemy.GetComponent<SmallEnemyHealth>().TakeStunnedDamage(playerStunningDamage);
            }
            if (enemy.GetComponent<NormalEnemyHealth>() != null)
            {
                
                enemy.GetComponent<NormalEnemyHealth>().TakeStunnedDamage(playerStunningDamage);
            }

            if (enemy.GetComponent<Fracture>() != null)
            {
                enemy.GetComponent<Fracture>().BreakObject();
                
            }

        }
    }

    IEnumerator UpdateEnergyUI()
    {
        currentBarValue = (currentEnergy * 1) / maxEnergy;

        playerEnergyBar.localScale = new Vector3(currentBarValue, 1, 1);

        yield return new WaitForSecondsRealtime(barBackgroundWaitTime);

        playerEnergyBackgroundBar.DOScale(new Vector3(currentBarValue, 1, 1), barBackgroundAnimationTime).SetEase(Ease.Linear);
    }

    public void EnergyPickup(float energy)
    {
        currentEnergy += energy;
        StartCoroutine(UpdateEnergyUI());

        if(currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }

    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }
}
