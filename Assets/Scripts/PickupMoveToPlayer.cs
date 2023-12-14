using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupMoveToPlayer : MonoBehaviour
{
    bool canMoveToPlayer;
    Transform playerTransform;
    public float timeUntilCanMoveToPlayer;
    public float moveSpeed;
    Vector3 playerPosition;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canMoveToPlayer = false;
        StartCoroutine(CanMove());
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMoveToPlayer) return;
        else
        {
            if (playerPosition == null) return;
            else
            {
                rb.useGravity = false;
                playerPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
                MoveToPlayer(playerPosition);
            }
        }

        moveSpeed += Time.deltaTime;
    }

    IEnumerator CanMove()
    {
        canMoveToPlayer = false;
        yield return new WaitForSecondsRealtime(timeUntilCanMoveToPlayer);
        canMoveToPlayer=true;
    }

    public void MoveToPlayer(Vector3 targetPosition)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        else
        {
            playerTransform = other.transform;
        }
    }
}
