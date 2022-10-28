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

    private void Update()
    {
        startButton.onClick.AddListener(OnStartClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);

    }

    void OnStartClicked ()
    {
        Debug.Log("Start Button Clicked");
        SceneManager.LoadScene(1);
    }

    void OnSettingsClicked ()
    {
        Debug.Log("Settings Button Clicked");
    }

    void OnQuitClicked ()
    {
        Debug.Log("Quit Button Clicked");
    }
}
