using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] Animator anim;

    [SerializeField] int levelNum;
    [SerializeField] int levelSceneNum;
    [SerializeField] float parTimeNum;

    public TMP_Text levelTitle;
    public TMP_Text fishCollected;
    public TMP_Text bestCompletionTime;
    public TMP_Text parTime;

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Button[] levelButtons;

    [SerializeField] float[] parTimes;

    private void Start()
    {
        for (int i = 1; i <= 10; i++)
        {
            if (!PlayerPrefs.HasKey(LevelParTimeString(i)) || PlayerPrefs.HasKey(LevelParTimeString(i)) && PlayerPrefs.GetFloat(LevelParTimeString(i)) != parTimes[i])
                PlayerPrefs.SetFloat(LevelParTimeString(i), parTimes[i]);
            else return;
            if (!PlayerPrefs.HasKey(HasCompletedLevel(i)))
                PlayerPrefs.SetInt(HasCompletedLevel(i), 0);
            else return;
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
        anim.SetBool("IsActive", false);
        scrollRect.enabled = true;
        foreach (Button button in levelButtons)
        {
            button.enabled = true;
        }
    }

    public void LevelSelect(int _levelSceneNum)
    {
        levelSceneNum = _levelSceneNum;
        levelNum = _levelSceneNum - 1;
        anim.SetBool("IsActive", true);
        scrollRect.enabled = false;
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
        PlayerPrefs.SetInt("CurrentLevel", levelNum);
        SceneManager.LoadScene(levelSceneNum);
    }
}
