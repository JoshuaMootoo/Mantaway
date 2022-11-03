using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void OnLoadScene(Object sceneName)
    {
        SceneManager.LoadScene(sceneName.name);
    }
}
