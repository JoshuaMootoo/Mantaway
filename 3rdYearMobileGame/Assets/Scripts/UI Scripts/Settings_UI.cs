using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings_UI : MonoBehaviour
{
    public AudioMixer audioMixer;
    AudioManager audioManager;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider soundSlider;

    public static Settings_UI instance;
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        if (PlayerPrefs.HasKey("MasterVolume")) masterSlider.value = PlayerPrefs.GetFloat("MasterVolume"); 
        else { 
            masterSlider.value = 1; 
            PlayerPrefs.SetFloat("MasterVolume", 1); 
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            musicSlider.value = 1;
            PlayerPrefs.SetFloat("MusicVolume", 1);
        }
        if (PlayerPrefs.HasKey("SoundVolume"))
        {
            soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        }
        else
        {
            soundSlider.value = 1;
            PlayerPrefs.SetFloat("SoundVolume", 1);
        }

        Debug.Log(PlayerPrefs.GetFloat("MasterVolume"));
        Debug.Log(PlayerPrefs.GetFloat("MusicVolume"));
        Debug.Log(PlayerPrefs.GetFloat("SoundVolume"));
    }

    public void SetMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        Debug.Log(PlayerPrefs.GetFloat("MasterVolume"));
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
    public void SetSoundVolume(float sliderValue)
    {
        audioMixer.SetFloat("SoundVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SoundVolume", sliderValue);
    }
    public void OtherButtonPress()
    {
        audioManager.Play("ButtonPress");
    }
}
