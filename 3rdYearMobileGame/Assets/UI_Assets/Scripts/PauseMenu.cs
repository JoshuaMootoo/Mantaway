using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject gameHUD, pauseMenu;

    private void Start()
    {
        gameHUD = GameObject.FindGameObjectWithTag("HUD");
        pauseMenu = transform.GetChild(0).gameObject;
    }

    public void PauseButton()
    {
        Time.timeScale = 0;
        gameHUD.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        gameHUD.SetActive(true);
        pauseMenu.SetActive(false);
    }
    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
