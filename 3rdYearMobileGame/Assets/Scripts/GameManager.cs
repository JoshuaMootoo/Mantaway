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

    public void GameOver()
    {
        if(gameOver == false)
        {
            gameOver = true;
            Debug.Log("GameOver");
            GameOverPanel.SetActive(true);
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


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        //QualitySettings.vSyncCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
