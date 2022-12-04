using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    FishCount fishCount;
    PlayerController playerController;

    [Header("UI Panels")]
    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject endGameUI;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject settingsUI;

    [Header("End Game UI Variables")]
    public TMP_Text endLevelText;
    public TMP_Text starRatingText;
    public GameObject[] stars = new GameObject[3];
    public TMP_Text fishCountText;
    public TMP_Text completionTimeText;
    public GameObject nextLevelButton;

    public float endGameTime;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        fishCount = FindObjectOfType<FishCount>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void DisplayTimeInMin(float _endGameTime)
    {
        float mins = Mathf.FloorToInt(_endGameTime / 60);
        float secs = Mathf.FloorToInt(_endGameTime % 60);
        completionTimeText.text = string.Format("Completion Time - {0:00}:{1:00}", mins, secs);
    }

    public void StarReward(int whatStar, bool hasAquired)
    {
        stars[whatStar].SetActive(hasAquired);
    }

    public void EndGame(bool hasGameEnded)
    {
        gameplayUI.SetActive(!hasGameEnded);
        endGameUI.SetActive(hasGameEnded);

        DisplayTimeInMin(endGameTime);
        fishCountText.text = string.Format("Fish Collected - {0:00}/{1:00}", playerController.foundFish, fishCount.maxFish);

        if (gameManager.isGameOver)
        {
            endLevelText.text = "Try Again";
            starRatingText.text = "";
            //  set all stars aquired to false for level
            //  add fish collected to text
            nextLevelButton.SetActive(false);
        }
        if (gameManager.isLevelComplete)
        {
            endLevelText.text = "Level Complete";
            starRatingText.text = "";
            //  check what stars have been aquired and activate them;
            //  add fish collected to text
            nextLevelButton.SetActive(true);
        }
    }

    //------------------------------------------------------ ADDED TO BUTTONS IN ENGINE ------------------------------------------------------

    public void PauseMenu(bool isPaused)
    {
        gameplayUI.SetActive(!isPaused);
        pauseMenuUI.SetActive(isPaused);
        if (isPaused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void Settings(bool isActve)
    {
        settingsUI.SetActive(isActve);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void QuitGame(bool mainMenu)
    {
        if (mainMenu) SceneManager.LoadScene("MainMenu");
        else SceneManager.LoadScene("LevelSelect");
        Time.timeScale = 1;
    }
}
