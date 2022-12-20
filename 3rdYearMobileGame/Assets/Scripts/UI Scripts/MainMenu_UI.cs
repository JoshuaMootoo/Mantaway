using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    AudioManager audioManager;
    public Animator anim;
    public float waitTime = 1;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OnLoadScene(int sceneNumber)
    {
        audioManager.Play("ButtonPress");
        StartCoroutine(LoadScene(sceneNumber));
    }

    public void OnQuitClicked ()
    {
        audioManager.Play("ButtonPress");
        //Debug.Log("Quit Button Clicked");
        Application.Quit();
    }

    IEnumerator LoadScene(int sceneNum)
    {
        anim.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(waitTime);
        SceneManager.LoadScene(sceneNum);
    }
}
