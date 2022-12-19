using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    public Animator anim;
    public float waitTime = 1;

    public void OnLoadScene(int sceneNumber)
    {
        StartCoroutine(LoadScene(sceneNumber));
    }

    public void OnQuitClicked ()
    {
        //Debug.Log("Quit Button Clicked");
        Application.Quit();
    }

    IEnumerator LoadScene(int sceneNum)
    {
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneNum);
    }
}
