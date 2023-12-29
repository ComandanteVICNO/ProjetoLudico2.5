using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenu;
    public GameObject healthUI;
    public GameObject energyUI;
    public GameObject dialogUI;

    [Header("Options References")]
    public Slider masterVolumeSlider;
    public Slider musicSlider;
    public Slider soundFxSlider;
    public AudioMixer gameAudioMixer;

    [Header ("Player Prefs Strings")]
    

    [Header("Options Values")]
    public float masterVolume;
    public float musicVolume;
    public float soundfxVolume;

    bool isPaused;

    private void Awake()
    {
        SetUiVisibility();
        
    }
    void Start()
    {
        isPaused = false;
        GetVolumePlayerPrefs();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPauseButton();
    }

    void SetUiVisibility()
    {
        pauseMenu.SetActive(false); 
        healthUI.SetActive(true);
        energyUI.SetActive(true);
    }

    void GetVolumePlayerPrefs()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            masterVolume = PlayerPrefs.GetFloat("MasterVolume");
            SetMasterVolume(masterVolume);
            masterVolumeSlider.value = masterVolume;
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            SetMusicVolume(musicVolume);
            musicSlider.value = musicVolume;
        }

        if (PlayerPrefs.HasKey("FxVolume"))
        {
            soundfxVolume = PlayerPrefs.GetFloat("FxVolume");
            SetSoundFxVolume(soundfxVolume);
            soundFxSlider.value = soundfxVolume;
        }
    }

    public void SetMasterVolume(float sliderValue)
    {
        gameAudioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", (float)sliderValue);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float sliderValue)
    {
        gameAudioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", (float)sliderValue);
        PlayerPrefs.Save();
    }

    public void SetSoundFxVolume(float sliderValue)
    {
        gameAudioMixer.SetFloat("FxVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("FxVolume", (float)sliderValue);
        PlayerPrefs.Save();
    }

    void CheckForPauseButton()
    {
        if (UserInput.instance.controls.Player.Pause.WasPressedThisFrame())
        {
            if (!isPaused)
            {
                isPaused = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                isPaused = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
    public void ResumeButton()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

}
