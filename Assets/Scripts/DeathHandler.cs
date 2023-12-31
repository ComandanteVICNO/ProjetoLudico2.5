using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using TMPro;

public class DeathHandler : MonoBehaviour
{
    public static DeathHandler instance;

    public PlayerHealth health;

    public bool isDead;
    public bool isRevived;
    public bool hasVignette;
    bool wasUIHidden = false;
    bool isWaitingForKeyDown = false;
    bool wasDeathPerformed = false;
    bool wasRevivePeromerd = false;
    public bool canBeAttacked = true;

    [Header("Fx Stuff")]
    public VolumeProfile fxVolume;
    public Vignette vignette;
    public float originalVignetteValue;
    public float deathVignetteValue;
    public float vignetteTransitionDuration;

    [Header("UI Stuff")]
    public GameObject healthUI;
    public GameObject energyUI;
    public GameObject dashUI;
    public GameObject deathUI;
    public TMP_Text title1;
    public TMP_Text title2;
    public float titleTransitionTime;
    public float timeUntilTitle1;
    public float timeUntilTitle2;


    void Start()
    {
        instance = this;
        isDead = false;
        isRevived = false;

        GetGlobalVignette();

    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerHealthStatus();
        GetGlobalVignette();
        CheckForInput();
        DoDeath();
        UndoDeath();
    }

    public void CheckPlayerHealthStatus()
    {
        if (health.currentHealth <= 0)
        {
            isDead = true;
            
        }
        else
        {
            isDead = false;
            
        }
    }

    public void GetGlobalVignette()
    {
        Vignette globalVignette;
        if (fxVolume.TryGet<Vignette>(out globalVignette))
        {
            vignette = globalVignette;
            if (vignette.active)
            {
                hasVignette = true;
            }
            else if (!vignette.active)
            {
                hasVignette = false;
            }
        }
    }


    public void HideUI()
    {
        if (isDead)
        {
            healthUI.SetActive(false);
            energyUI.SetActive(false);
            dashUI.SetActive(false);
            deathUI.SetActive(true);

            title1.DOFade(0f, 0.01f);
            title2.DOFade(0f, 0.01f);
        }
    }
    


    public void DoDeath()
    {
        if (!isDead) return;
        if (wasDeathPerformed) return;
        canBeAttacked = false;
        CamManager.current.FocusOnDeathCam();
        PlayerMovController.instance.canMove = false;
        PlayerAttack.current.isAttackAllow = false;
        HideUI();
        if (hasVignette)
        {
            StartCoroutine(VignetteTransition(originalVignetteValue, deathVignetteValue, vignetteTransitionDuration));
        }
        else
        {
            vignette.active = true;
            StartCoroutine(VignetteTransition(0f, deathVignetteValue, vignetteTransitionDuration));
        }
        
        StartCoroutine(ShowFirstLine());
        StartCoroutine(ShowSecondLine());

        wasDeathPerformed = true;
    }

    public void UndoDeath()
    {
        if(isDead) return;
        if (!isRevived) return;
        if(wasRevivePeromerd) return;
        StartCoroutine(RevivePlayer());
        wasRevivePeromerd = true;
    }

    IEnumerator VignetteTransition(float startValue, float finalValue, float transitionTime)
    {
        float elapsedTime = 0f;

        while(elapsedTime< transitionTime)
        {
            float time = elapsedTime / transitionTime;
            vignette.intensity.value = Mathf.Lerp(startValue, finalValue, time);

            elapsedTime += Time.deltaTime;

            yield return null;

        }

        vignette.intensity.value = finalValue;
    }

    IEnumerator ShowFirstLine()
    {
        
        yield return new WaitForSecondsRealtime(timeUntilTitle1);

        title1.DOFade(1f, titleTransitionTime);

    }

    IEnumerator ShowSecondLine()
    {

        yield return new WaitForSecondsRealtime(timeUntilTitle2);

        title2.DOFade(1f, titleTransitionTime);

        isWaitingForKeyDown = true;


    }

    public void CheckForInput()
    {
        if (!isWaitingForKeyDown) return;
        if (Input.anyKeyDown)
        {
            RestoreHealth();    
            isWaitingForKeyDown=false;
            isRevived = true;
        }
    }

    IEnumerator RevivePlayer()
    {
        title1.DOFade(0f, titleTransitionTime);
        title2.DOFade(0f, titleTransitionTime);

        yield return new WaitForSecondsRealtime(titleTransitionTime);

        CamManager.current.UnfocusOnDeathCam();



        if (hasVignette)
        {
            StartCoroutine(VignetteTransition(deathVignetteValue, originalVignetteValue, vignetteTransitionDuration));
        }
        else
        {
            
            StartCoroutine(VignetteTransition(deathVignetteValue, 0f, vignetteTransitionDuration));
        }

        yield return new WaitForSecondsRealtime(vignetteTransitionDuration);

        if (!hasVignette)
        {
            vignette.active = false;
        }

        ResetBool();
        PlayerMovController.instance.canMove = true;
        PlayerAttack.current.isAttackAllow = true;
        canBeAttacked = true;
        StartCoroutine(health.UpdateHealth());
        HideUI();

        healthUI.SetActive(true);
        energyUI.SetActive(true);
        dashUI.SetActive(true);
        deathUI.SetActive(false);
    }

    public void RestoreHealth()
    {
        health.currentHealth = health.maxHealth;
    }

    public void ResetBool()
    {
        isDead = false;
        isRevived = false;
        wasDeathPerformed = false;
        wasRevivePeromerd = false;
    }
}
