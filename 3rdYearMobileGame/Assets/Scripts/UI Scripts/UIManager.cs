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
    AudioManager audioManager;

    [Header("Scene Transition Variables")]
    public Animator anim;
    public float waitTime = 1;

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
    public int levelNum;
    public int starCounter = 0;

    [Header("Fish Counter")]
    public int collectedfish;
    public int maxFish;

    private void Start()
    {
        starCounter = 0;

        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();
        audioManager = FindObjectOfType<AudioManager>();


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

        levelNum = (SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void DisplayTimeInMin(float _endGameTime)
    {
        float mins = Mathf.FloorToInt(_endGameTime / 60);
        float secs = Mathf.FloorToInt(_endGameTime % 60);
        completionTimeText.text = string.Format("Completion Time - {0:00}:{1:00}", mins, secs);
    }

    public void HasAchievedStar()
    {
        if (PlayerPrefs.GetInt(HasCompletedLevel(levelNum)) == 1)
        {
            StarReward(0, true);
            starCounter += 1;
        }
        else StarReward(0, false);
        if (PlayerPrefs.GetFloat(EndLevelTime(levelNum)) < PlayerPrefs.GetFloat(LevelParTimeString(levelNum)))
        {
            StarReward(1, true);
            starCounter += 1;
        }
        else StarReward(1, false);
        if (PlayerPrefs.GetInt(FishCollected(levelNum)) == PlayerPrefs.GetInt(TotalFish(levelNum)))
        {
            StarReward(2, true);
            starCounter += 1;
        }
        else StarReward(2, false);
    }

    public void StarReward(int whatStar, bool hasAquired)
    {
        stars[whatStar].SetActive(hasAquired);
    }

    public void StarRewardText ()
    {
        if (starCounter == 0) starRatingText.text = "";
        if (starCounter == 1) starRatingText.text = "Well done!";
        if (starCounter == 2) starRatingText.text = "Nice work!";
        if (starCounter == 3) starRatingText.text = "Perfect!";
    }

    public void EndGamePlayerPrefs()
    {

        if (!PlayerPrefs.HasKey(EndLevelTime(levelNum)) ||
            PlayerPrefs.HasKey(EndLevelTime(levelNum)) && endGameTime < PlayerPrefs.GetFloat(EndLevelTime(levelNum)))
        {
            PlayerPrefs.SetFloat(EndLevelTime(levelNum), endGameTime);
        }

        if (!PlayerPrefs.HasKey(FishCollected(levelNum)) ||
            PlayerPrefs.HasKey(FishCollected(levelNum)) && collectedfish > PlayerPrefs.GetInt(FishCollected(levelNum)))
        {
            PlayerPrefs.SetInt(FishCollected(levelNum), collectedfish);
        }

        if (!PlayerPrefs.HasKey(TotalFish(levelNum)) || 
            PlayerPrefs.HasKey(TotalFish(levelNum)) && maxFish != PlayerPrefs.GetInt(TotalFish(levelNum)))
        {
            PlayerPrefs.SetInt(TotalFish(levelNum), maxFish);
        }
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
            restartButton.GetComponentInChildren<TMP_Text>().text = "Retry Level";


            //  set all stars aquired to false for level
            //  add fish collected to text
            nextLevelButton.SetActive(false);
        }
        if (gameManager.isLevelComplete)
        {
            endLevelText.text = "Level Complete";
            PlayerPrefs.SetInt(HasCompletedLevel(levelNum), 1);
            EndGamePlayerPrefs();
            HasAchievedStar();
            restartButton.GetComponentInChildren<TMP_Text>().text = "Restart Level";
            //  check what stars have been aquired and activate them;
            //  add fish collected to text
            nextLevelButton.SetActive(true);
        }

        StarRewardText();
    }

    //---------------------------------------------------------- CALLED BY PLAYER ------------------------------------------------------------

    public void SetFishCount(int foundFish)
    {
        collectedfish = foundFish;
    }

    //--------------------------------------------------------- PLAYERPREFS STRINGS ----------------------------------------------------------

    string EndLevelTime(int level)
    {
        string endLevelTimeString;
        endLevelTimeString = "EndLevelTimeLevel" + level;
        return endLevelTimeString;
    }

    string FishCollected(int level)
    {
        string fishCollectedString;
        fishCollectedString = "FishCollectedLevel" + level;
        return fishCollectedString;
    }

    string TotalFish(int level)
    {
        string totalFishString;
        totalFishString = "TotalFishLevel" + level;
        return totalFishString;
    }

    string LevelParTimeString(int level)
    {
        string levelParTimeString;
        levelParTimeString = "ParTimeLevel" + level;
        return levelParTimeString;
    }

    string HasCompletedLevel(int level)
    {
        string hasCompletedLevelString;
        hasCompletedLevelString = "HasCompletedLevel" + level;
        return hasCompletedLevelString;
    }

    //------------------------------------------------------ ADDED TO BUTTONS IN ENGINE ------------------------------------------------------

    public void PauseMenu(bool isPaused)
    {
        audioManager.Play("ButtonPress");
        gameplayUI.SetActive(!isPaused);
        pauseMenuUI.SetActive(isPaused);
        if (isPaused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void Settings(bool isActve)
    {
        audioManager.Play("ButtonPress");
        settingsUI.SetActive(isActve);
    }

    public void NextLevel()
    {
        audioManager.Play("ButtonPress");
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        Time.timeScale = 1;
    }

    public void Restart()
    {
        audioManager.Play("ButtonPress");
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
        Time.timeScale = 1;
    }

    public void QuitGame(bool mainMenu)
    {
        audioManager.Play("ButtonPress");
        if (mainMenu) StartCoroutine(LoadScene(0));
        else StartCoroutine(LoadScene(1));
        Time.timeScale = 1;
    }
    public void OtherButtonPress()
    {
        audioManager.Play("ButtonPress");
    }

    IEnumerator LoadScene(int sceneNum)
    {
        anim.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(waitTime);
        SceneManager.LoadScene(sceneNum);
    }
}
