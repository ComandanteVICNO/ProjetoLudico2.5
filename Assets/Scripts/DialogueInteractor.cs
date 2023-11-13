using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogueInteractor : MonoBehaviour
{
    [Multiline]
    public string[] dialogue;
    public TMP_Text hintText;
    public TMP_Text dialogueText;
    private int currentIndex = 0;
    public DialogueWriter writer;
    

    bool firstMessageRead;
    bool playerInZone;

    private void Awake()
    {
        writer = FindObjectOfType<DialogueWriter>();
        dialogueText = writer.dialogueText;
        writer.HideDialogue();
        HideHint();
        firstMessageRead = false;
    }

    void Update()
    {
        if (playerInZone && UserInput.instance.controls.Player.Interact.WasPressedThisFrame())
        {

            CamManager.current.FocusCamera(transform, transform);

            HideHint();
            

            if(!firstMessageRead)
            {
                DialogueWriter.instance.ShowDialogue(); 
                ShowMessageAtIndex(currentIndex);
                firstMessageRead=true;
            }
            else
            {
                DialogueWriter.instance.ShowDialogue();

                ShowNextMessage();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        

        playerInZone = true;
        ShowHint();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        firstMessageRead = false;
        playerInZone = false;
        HideHint();

        CamManager.current.UnfocusCamera();
    }

    void ShowHint()
    {
        hintText.enabled = true;   
    }

    void HideHint()
    {
        hintText.enabled = false;
    }

    

    void ShowMessageAtIndex(int index)
    {
        if (index >= 0 && index < dialogue.Length)
        {
            dialogueText.text = dialogue[index];
        }
    }

    void ShowNextMessage()
    {
        
        currentIndex++;
        ShowMessageAtIndex(currentIndex);

        
        if (currentIndex >= dialogue.Length)
        {
            EndDialog();
        }
    }

    void EndDialog()
    {
        currentIndex = 0;
        DialogueWriter.instance.HideDialogue();
        HideHint();
        playerInZone = false;
        CamManager.current.UnfocusCamera();
    }
}
