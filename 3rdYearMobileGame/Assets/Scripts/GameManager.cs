using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameOver = false;
    bool levelComplete = false;
    public float restarDelay = 1f;
    
    public void GameOver()
    {
        if(gameOver == false)
        {
            gameOver = true;
            Debug.Log("GameOver");
            Invoke("Restart", restarDelay);
        }
    }

    public void LevelComplete()
    {
        if(levelComplete == false)
        {
            levelComplete = true;
            Debug.Log("Level Complete");
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
