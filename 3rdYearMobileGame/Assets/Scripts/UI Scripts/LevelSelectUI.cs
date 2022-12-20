using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    AudioManager audioManager;

    [Header("Scene Transition Variables")]
    public Animator animST;
    public float waitTime = 1;

    [SerializeField] Animator anim;

    [SerializeField] int levelNum;
    [SerializeField] int levelSceneNum;
    [SerializeField] float parTimeNum;

    public TMP_Text levelTitle;
    public TMP_Text fishCollected;
    public TMP_Text bestCompletionTime;
    public TMP_Text parTime;

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Button[] levelButtons = new Button[10];

    [SerializeField] float[] parTimes = new float[10];

    public GameObject[] stars = new GameObject[3];

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        for (int i = 1; i <= 10; i++)
        {
            //  Sets all the par times for each level
            PlayerPrefs.SetFloat(LevelParTimeString(i), parTimes[i-1]);
            //  if the variable doesnt exist, Sets the level complete variable to 0 meaning the level isnt completed
            if (!PlayerPrefs.HasKey(HasCompletedLevel(i)))
                PlayerPrefs.SetInt(HasCompletedLevel(i), 0);
        }
    }
    private void Update()
    {
        levelTitle.text = "Level " + levelNum;
        if (PlayerPrefs.HasKey(FishCollectedString(levelNum)))
            fishCollected.text = string.Format("Fish Collected - {0:00}/{1:00}", PlayerPrefs.GetInt(FishCollectedString(levelNum)), PlayerPrefs.GetInt(TotalFishString(levelNum)));
        else fishCollected.text = "Fish Collected - ??/??";
        if (PlayerPrefs.HasKey(EndLevelTimeString(levelNum))) bestCompletionTime.text = "Best Completion Time - " + DisplayTimeInMin(PlayerPrefs.GetFloat(EndLevelTimeString(levelNum)));
        else bestCompletionTime.text = "Best Completion Time - --:--";

        parTime.text = "Par Time - " + DisplayTimeInMin(PlayerPrefs.GetFloat(LevelParTimeString(levelNum)));
    }

    private string DisplayTimeInMin(float _endGameTime)
    {
        float mins = Mathf.FloorToInt(_endGameTime / 60);
        float secs = Mathf.FloorToInt(_endGameTime % 60);
        return string.Format("{0:00}:{1:00}", mins, secs);
    }

    public void HasAchievedStar()
    {
        Debug.Log(PlayerPrefs.GetInt(HasCompletedLevel(levelNum)));
        Debug.Log(PlayerPrefs.GetFloat(EndLevelTimeString(levelNum)));
        Debug.Log(PlayerPrefs.GetFloat(FishCollectedString(levelNum)));
        Debug.Log(PlayerPrefs.GetFloat(TotalFishString(levelNum)));

        if (PlayerPrefs.GetInt(HasCompletedLevel(levelNum)) == 1) 
        {
            //  Star 1 requires you to complete the level
            StarReward(0, true); 
            //  Star 2 requires you to beat the par time
            if (PlayerPrefs.GetFloat(EndLevelTimeString(levelNum)) < PlayerPrefs.GetFloat(LevelParTimeString(levelNum))) 
                StarReward(1, true); 
            else StarReward(1, false);
            //  Star 3 requires you to collect all the fish
            if (PlayerPrefs.GetInt(FishCollectedString(levelNum)) == PlayerPrefs.GetInt(TotalFishString(levelNum))) 
                StarReward(2, true); 
            else StarReward(2, false);
        } else StarReward(0, false);
    }

    public void StarReward(int whatStar, bool hasAquired)
    {
        stars[whatStar].SetActive(hasAquired);
    }

    //--------------------------------------------------------- PLAYERPREFS STRINGS ----------------------------------------------------------

    string EndLevelTimeString(int level)
    {
        string endLevelTimeString;
        endLevelTimeString = "EndLevelTimeLevel" + level;
        return endLevelTimeString;
    }

    string FishCollectedString(int level)
    {
        string fishCollectedString;
        fishCollectedString = "FishCollectedLevel" + level;
        return fishCollectedString;
    }

    string TotalFishString(int level)
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

    public void BackButton()
    {
        audioManager.Play("ButtonPress");
        anim.SetBool("IsActive", false);
        scrollRect.enabled = true;
        foreach (Button button in levelButtons)
        {
            button.enabled = true;
        }
    }

    public void LevelSelect(int _levelSceneNum)
    {
        audioManager.Play("ButtonPress");
        levelSceneNum = _levelSceneNum;
        levelNum = _levelSceneNum - 1;
        anim.SetBool("IsActive", true);
        scrollRect.enabled = false;
        HasAchievedStar();
        foreach (Button button in levelButtons)
        {
            button.enabled = false;
        }
    }

    public void LevelParTime(float _parTime)
    {
        if (!PlayerPrefs.HasKey(LevelParTimeString(levelNum)) || PlayerPrefs.HasKey(LevelParTimeString(levelNum)) && PlayerPrefs.GetFloat(LevelParTimeString(levelNum)) != _parTime) 
            PlayerPrefs.SetFloat(LevelParTimeString(levelNum), _parTime);
    }

    public void StartGameButton()
    {
        audioManager.Play("ButtonPress");
        PlayerPrefs.SetInt("CurrentLevel", levelNum);
        StartCoroutine(LoadScene(levelSceneNum));
    }

    IEnumerator LoadScene(int sceneNum)
    {
        animST.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(waitTime);
        SceneManager.LoadScene(sceneNum);
    }
}
