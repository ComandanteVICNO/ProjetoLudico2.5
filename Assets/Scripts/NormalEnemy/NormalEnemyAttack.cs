using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyAttack : MonoBehaviour
{
    public float attackCooldown;
    public float damage;
    private float cooldownTimer = Mathf.Infinity;
    public LayerMask player;


    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        //attack only when player is in sight?
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                //attack
            }
        }
    }

    private bool PlayerInSight()
    {

        return false;
    }
}
