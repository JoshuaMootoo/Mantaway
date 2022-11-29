using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isGameOver;
    public bool isLevelComplete;

    public float timerValue;

    AudioManager audioManager;
    UIManager uIManager;
    //public static GameManager instance;

    private void Awake()
    {
        //if (instance == null)
        //    instance = this;
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        //DontDestroyOnLoad(gameObject);

        audioManager = FindObjectOfType<AudioManager>();
        uIManager = FindObjectOfType<UIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        //QualitySettings.vSyncCount = 1;

        timerValue = 0;

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
           
        }
    }

    private void Update()
    {
        timerValue += Time.deltaTime;

    }    
    
    public void EndGame(bool _isGameOver, bool _isLevelComplete)
    {
        isGameOver = _isGameOver;
        isLevelComplete = _isLevelComplete;

        Time.timeScale = 0;

        uIManager.endGameTime = timerValue;
        uIManager.EndGame(true);

        if (isGameOver)
        {
            Debug.Log("GameOver");
            audioManager.Play("LevelFail");
        } 
        if (isLevelComplete)
        {
            Debug.Log("Level Complete");
            audioManager.Play("LevelFail");
        }        
    }
}
