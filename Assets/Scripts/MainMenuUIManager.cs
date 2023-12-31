using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Options References")]
    public Slider masterVolumeSlider;
    public Slider musicSlider;
    public Slider soundFxSlider;
    public AudioMixer gameAudioMixer;
    


    [Header("Options Values")]
    public float masterVolume;
    public float musicVolume;
    public float soundfxVolume;

    [Header("PostFX")]
    public Toggle filmGrainToggle;
    public Toggle vignetteToggle;
    bool hasFilmGrain;
    bool hasVignette;

    Volume volume;

    // Start is called before the first frame update
    void Start()
    {
        GetVolumePlayerPrefs();
        GetPostFxPlayerPrefs();
    }

    // Update is called once per frame
    void Update()
    {
        
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



    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void CloseButton()
    {
        Application.Quit();
    }

    public void GetPostFxPlayerPrefs()
    {
        //Check for Filmgrain
        if (PlayerPrefs.HasKey("FilmGrainEnabled"))
        {
            
            int filmGrainEnabled = PlayerPrefs.GetInt("FilmGrainEnabled");
            if(filmGrainEnabled == 0)
            {
                hasFilmGrain = false;
                filmGrainToggle.isOn = false;
            }
            else if(filmGrainEnabled == 1)
            {
                hasFilmGrain = true;
                filmGrainToggle.isOn = true;
            }
        }
        else
        {
            
            hasFilmGrain = true;
        }

        //check for vignette
        if (PlayerPrefs.HasKey("Vignette"))
        {

            int vignetteEnabled = PlayerPrefs.GetInt("Vignette");
            if (vignetteEnabled == 0)
            {
                hasVignette = false;
                vignetteToggle.isOn = false;
            }
            else if (vignetteEnabled == 1)
            {
                hasVignette = true;
                vignetteToggle.isOn = true;
            }
        }
        else
        {

            hasVignette = true;
        }
    }

    public void ToggleFilmGrain(bool active)
    {
        if(active == true)
        {
            PlayerPrefs.SetInt("FilmGrainEnabled", 1);
        }
        else if (active == false)
        {
            PlayerPrefs.SetInt("FilmGrainEnabled", 0);
        }
        PlayerPrefs.Save();
    }

    public void ToggleVignette(bool active)
    {
        if (active == true)
        {
            PlayerPrefs.SetInt("Vignette", 1);
        }
        else if (active == false)
        {
            PlayerPrefs.SetInt("Vignette", 0);
        }
        PlayerPrefs.Save();
    }
}
