using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button quitButton;



    public void OnStartClicked ()
    {
        //Debug.Log("Start Button Clicked");
        SceneManager.LoadScene(1);
    }

    public void OnSettingsClicked ()
    {
        Debug.Log("Settings Button Clicked");
    }

    public void OnQuitClicked ()
    {
        Debug.Log("Quit Button Clicked");
        Application.Quit();
    }
}
