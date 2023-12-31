using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class ChangeSceneArea : MonoBehaviour
{
    public Transform playerTrasform;
    public PlayerMovController movController;
    public Rigidbody rb;
    public float waitTime;
  

    public Image transitionImage;

    public GameObject player;

    Color32 blackColor = new Color32(0,0,0,255);
    Color32 noColor = new Color32(0, 0, 0, 0);

    public float colorFadeTime;

    //temporary cause it's still no changing scenes
    public Transform teleportLocation;



    // Update is called once per frame
    void Update()
    {
        
    }

    public void CancelMovement()
    {
        movController.canMove = false;
        movController.canJump = false;

        rb.velocity = Vector3.zero;
    }

    public void AllowMovement()
    {
        movController.canMove = true;
        movController.canJump = true;

    }

    public void MovePlayer()
    {
        playerTrasform.position = teleportLocation.position;
    }


    IEnumerator Teleport()
    {
        CancelMovement();
        MovePlayer();

        yield return new WaitForSecondsRealtime(waitTime);

        AllowMovement();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        {
            player = other.gameObject;
            rb = other.GetComponent<Rigidbody>();
            playerTrasform = other.transform;
            movController = FindAnyObjectByType<PlayerMovController>();
            StartCoroutine(StartTransition());
        }
    }

    IEnumerator StartTransition()
    {
        CancelMovement();

        
        transitionImage.DOColor(blackColor, colorFadeTime).SetEase(Ease.Linear); ;
        

        yield return new WaitForSecondsRealtime(colorFadeTime);

        MovePlayer();
        StartCoroutine(EndTrasition());

    }

    IEnumerator EndTrasition()
    {
        transitionImage.DOColor(noColor, colorFadeTime).SetEase(Ease.Linear);

        yield return new WaitForSecondsRealtime(colorFadeTime);

        AllowMovement();
    }
}
