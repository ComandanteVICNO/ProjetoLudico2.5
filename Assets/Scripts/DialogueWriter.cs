using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DialogueWriter : MonoBehaviour
{
    public static DialogueWriter instance;

    public TMP_Text dialogueText;
    public GameObject UI;
    public GameObject uiCheckmark;
    public Transform uiCheckmarkTransform;
    public Vector3 uiCheckmarkOrigin;

    public float animationDuration;
    bool isAnimating = false;

    private void Awake()
    {
        instance = this;
        isAnimating = false;
    }

    private void Update()
    {
        if (!isAnimating)
        {
            StartCoroutine(CheckmarkAnimation());
        }
    }
    private void Start()
    {
        uiCheckmarkTransform = uiCheckmark.transform;
        
        uiCheckmarkOrigin = new Vector3(uiCheckmarkTransform.position.x, uiCheckmarkTransform.position.y, uiCheckmarkTransform.position.z);
    }
    public void ShowDialogue()
    {
        UI.SetActive(true);
        
    }

    public void HideDialogue()
    {
        UI.SetActive(false);
        
    }

    IEnumerator CheckmarkAnimation()
    {
        isAnimating = true;
        uiCheckmarkTransform.DOMove(new Vector3(uiCheckmarkTransform.position.x, uiCheckmarkTransform.position.y - 20, uiCheckmarkTransform.position.z), animationDuration);
        yield return new WaitForSecondsRealtime(animationDuration + 0.1f);
        isAnimating = false;
        uiCheckmarkTransform.position = uiCheckmarkOrigin;
    }
}
