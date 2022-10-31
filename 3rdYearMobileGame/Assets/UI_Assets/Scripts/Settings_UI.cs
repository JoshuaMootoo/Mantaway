using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings_UI : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMasterVolume (float volume)
    {
        //Debug.Log(volume);
        audioMixer.SetFloat("Master_EP", volume);
    }
    public void SetMusicVolume(float volume)
    {
        //Debug.Log(volume);
        audioMixer.SetFloat("Music_EP", volume);
    }
    public void SetSoundVolume(float volume)
    {
        //Debug.Log(volume);
        audioMixer.SetFloat("Sound_EP", volume);
    }
}
