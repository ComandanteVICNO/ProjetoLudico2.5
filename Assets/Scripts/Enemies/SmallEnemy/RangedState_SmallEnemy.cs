using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class RangedState_SmallEnemy : LogicMachineBehaviour<SmallEnemyLogicManager>
{
    public Transform playerTransform;
    public Vector3 directionToPlayer;
    private CancellationTokenSource cancellationTokenSource;
    bool canLaunch;

    public override void OnAwake()
    {
        
    }

    public override void OnEnter()
    {
        cancellationTokenSource = new CancellationTokenSource();
        canLaunch = true;
    }
    public override void OnUpdate()
    {
        StateUpdater();
        StopMovement();
        if (manager.detectPlayerSphere.isPlayerDetected)
        {
            GetPlayerTransform();
            FindDirectionToPlayer();
            LookAtPlayer();
            if (canLaunch)
            {
                Debug.Log("can lanche");
                FireProjectileCooldown();
            }
        }
    }

    public override void OnExit()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
        logicAnimator.SetBool("canRange", false);
    }
    void StopMovement()
    {
        manager.rb.velocity = Vector3.zero;
    }

    void LookAtPlayer()
    {
        Vector3 direction = playerTransform.position - transform.position;

        float dotProduct = Vector3.Dot(direction, transform.right);

        if (dotProduct > 0)
        {
            // Target is on the right
            manager.enemyTransform.localScale = new Vector3(-1, 1, 1);
        }
        else if (dotProduct < 0)
        {
            // Target is on the left
            manager.enemyTransform.localScale = new Vector3(1, 1, 1);
        }
    }

    void StateUpdater()
    {
        

        if (!manager.detectPlayerSphere.isPlayerDetected)
        {
            logicAnimator.SetBool("canRange", false);
        }

        if(manager.detectPlayerHitbox.isPlayerDetected)
        {
            logicAnimator.SetBool("canCharge", true);
        }
    }

    void GetPlayerTransform()
    {
        if (manager.detectPlayerSphere.playerTransform == null) return;

        playerTransform = manager.detectPlayerSphere.playerTransform;
    }
    
    void FindDirectionToPlayer()
    {
        if (playerTransform == null) return;
        else
        {
            directionToPlayer = playerTransform.position - transform.position;
        }
    }


    async void FireProjectileCooldown()
    {
        canLaunch = false;
        float time = manager.projectileCooldown;
        
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(time), cancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {

            return;
        }
        if (!active) return;
        InstantiateProjectile(directionToPlayer);
        canLaunch = true;
    }

    void InstantiateProjectile(Vector3 playerDir)
    {
        GameObject projectile = Instantiate(manager.projectile, transform.position, Quaternion.identity);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 normalizedPlayerDir = playerDir.normalized;

        
        float angle = Mathf.Atan2(normalizedPlayerDir.y, normalizedPlayerDir.x) * Mathf.Rad2Deg;

        
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        projectileRb.AddForce(normalizedPlayerDir * manager.projectileForce, ForceMode.Impulse);

        Debug.Log("Lanchado");
    }

}
