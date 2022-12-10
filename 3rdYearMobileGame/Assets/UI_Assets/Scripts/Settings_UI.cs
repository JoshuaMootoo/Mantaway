using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings_UI : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider soundSlider;

    public static Settings_UI instance;
    private void Awake()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");      //  Debug.Log(PlayerPrefs.GetFloat("MasterVolume"));
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");        //  Debug.Log(PlayerPrefs.GetFloat("MusicVolume"));
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");        //  Debug.Log(PlayerPrefs.GetFloat("SoundVolume"));
    }

    private void Update()
    { 
        audioMixer.SetFloat("Master_EP", masterSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);

        audioMixer.SetFloat("Music_EP", musicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);

        audioMixer.SetFloat("Sound_EP", soundSlider.value);
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
    }    
}
