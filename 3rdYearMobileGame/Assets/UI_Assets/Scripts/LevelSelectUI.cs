using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] Animator anim;

    [SerializeField]int levelNum;
    [SerializeField]int levelSceneNum;

    public TMP_Text levelTitle;
    public TMP_Text fishCollected;
    public TMP_Text bestCompletionTime;

    private void Update()
    {
        levelTitle.text = "Level " + levelNum;
        if (PlayerPrefs.HasKey(FishCollected(levelNum)))
            fishCollected.text = string.Format("Fish Collected - {0:00}/{1:00}", PlayerPrefs.GetInt(FishCollected(levelNum)), PlayerPrefs.GetInt(TotalFish(levelNum)));
        else fishCollected.text = "Fish Collected - ??/??";
        if (PlayerPrefs.HasKey(EndLevelTime(levelNum))) DisplayTimeInMin(PlayerPrefs.GetFloat(EndLevelTime(levelNum)));
        else bestCompletionTime.text = "Best Completion Time - --:--";
    }

    private void DisplayTimeInMin(float _endGameTime)
    {
        float mins = Mathf.FloorToInt(_endGameTime / 60);
        float secs = Mathf.FloorToInt(_endGameTime % 60);
        bestCompletionTime.text = string.Format("Best Completion Time - {0:00}:{1:00}", mins, secs);
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

    //------------------------------------------------------ ADDED TO BUTTONS IN ENGINE ------------------------------------------------------

    public void BackButton()
    {
        anim.SetBool("IsActive", false);
    }

    public void LevelSelect(int _levelSceneNum)
    {
        levelSceneNum = _levelSceneNum;
        levelNum = _levelSceneNum - 1;
        anim.SetBool("IsActive", true);
    }

    public void StartGameButton()
    {
        PlayerPrefs.SetInt("CurrentLevel", levelNum);
        SceneManager.LoadScene(levelSceneNum);
    }
}
