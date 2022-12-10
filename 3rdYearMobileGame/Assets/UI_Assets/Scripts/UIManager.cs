using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;
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
    public Slider fishCountSlider;
    public TMP_Text completionTimeText;
    public GameObject nextLevelButton;
    public Button restartButton;

    public float endGameTime;

    [Header("Fish Counter")]
    public int collectedfish;
    public int maxFish;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();


        foreach (FishController fish in FindObjectsOfType<FishController>())
        {
            maxFish += 1;
        }

        foreach (CrateController crate in FindObjectsOfType<CrateController>())
        {
            maxFish += crate.heldFish;
        }

        fishCountSlider.minValue = 0;
        fishCountSlider.maxValue = maxFish;
    }

    private void DisplayTimeInMin(float _endGameTime)
    {
        float mins = Mathf.FloorToInt(_endGameTime / 60);
        float secs = Mathf.FloorToInt(_endGameTime % 60);
        completionTimeText.text = string.Format("Completion Time - {0:00}:{1:00}", mins, secs);
    }

    public void SetFishCount(int foundFish)
    {
        collectedfish = foundFish;
    }

    public void StarReward(int whatStar, bool hasAquired)
    {
        stars[whatStar].SetActive(hasAquired);
    }

    public void EndGamePlayerPrefs()
    {
        string endGameTimeString = "EndLevelTimeLevel" + PlayerPrefs.GetInt("CurrentLevel");
        if (endGameTime < PlayerPrefs.GetFloat(endGameTimeString) && PlayerPrefs.HasKey(endGameTimeString) || !PlayerPrefs.HasKey(endGameTimeString))
            PlayerPrefs.SetFloat("EndLevelTimeLevel" + PlayerPrefs.GetInt("CurrentLevel"), endGameTime);

        string fishCollectedString = "FishCollectedLevel" + PlayerPrefs.GetInt("CurrentLevel");
        if (collectedfish > PlayerPrefs.GetInt(fishCollectedString) && PlayerPrefs.HasKey(fishCollectedString) || !PlayerPrefs.HasKey(fishCollectedString))
            PlayerPrefs.SetInt(fishCollectedString, collectedfish);

        string totalFishString = "TotalFishLevel" + PlayerPrefs.GetInt("CurrentLevel");
        if (!PlayerPrefs.HasKey(totalFishString))
            PlayerPrefs.SetInt(totalFishString, maxFish);
    }

    public void EndGame(bool hasGameEnded)
    {
        gameplayUI.SetActive(!hasGameEnded);
        endGameUI.SetActive(hasGameEnded);

        DisplayTimeInMin(endGameTime);
        fishCountText.text = string.Format("Fish Collected - {0:00}/{1:00}", playerController.foundFish, maxFish);
        fishCountSlider.value = collectedfish;

        if (gameManager.isGameOver)
        {
            endLevelText.text = "Try Again";
            starRatingText.text = "";
            restartButton.GetComponentInChildren<TMP_Text>().text = "Retry Level";


            //  set all stars aquired to false for level
            //  add fish collected to text
            nextLevelButton.SetActive(false);
        }
        if (gameManager.isLevelComplete)
        {
            endLevelText.text = "Level Complete";
            starRatingText.text = "";
            restartButton.GetComponentInChildren<TMP_Text>().text = "Restart Level";
            EndGamePlayerPrefs();
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
