using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField]
    private Button quitButton;

    public void OnQuitClicked ()
    {
        //Debug.Log("Quit Button Clicked");
        Application.Quit();
    }
}
