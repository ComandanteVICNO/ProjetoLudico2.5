using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public Transform originalTransform;
    public Transform targetTransform;

    public float closeAnimationSpeed;
    public float openAnimationSpeed;

    public Ease closeEase;
    public Ease openEase;

    private void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void CloseDoors()
    {
        transform.DOMove(targetTransform.position, closeAnimationSpeed) .SetEase(closeEase);
    }

    public void OpenDoors()
    {
        transform.DOMove(originalTransform.position, openAnimationSpeed).SetEase(openEase);
    }

}
