using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void OnLoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    
}
