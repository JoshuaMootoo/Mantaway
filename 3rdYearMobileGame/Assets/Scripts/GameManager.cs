using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameOver = false;
    bool levelComplete = false;
    public float restarDelay = 1f;

    public GameObject LevelCompletePanel;
    public GameObject GameOverPanel;

    AudioManager audioManager;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        audioManager = FindObjectOfType<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        //QualitySettings.vSyncCount = 1;


        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
           
        }
    }


    public void GameOver()
    {
        if(gameOver == false)
        {
            gameOver = true;
            Debug.Log("GameOver");
            GameOverPanel.SetActive(true);
            FindObjectOfType<AudioManager>().Play("LevelFail");
            Invoke("Restart", restarDelay);
        }
    }

    public void LevelComplete()
    {
        if(levelComplete == false)
        {
            levelComplete = true;
            Debug.Log("Level Complete");
            LevelCompletePanel.SetActive(true);
            FindObjectOfType<AudioManager>().Play("LevelComplete");
            Invoke("Exit", restarDelay);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }


   

    // Update is called once per frame
    void Update()
    {
        
    }


}
