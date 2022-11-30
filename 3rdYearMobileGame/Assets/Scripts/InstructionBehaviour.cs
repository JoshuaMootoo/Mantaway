using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject PauseUI;
    [SerializeField]
    GameObject GameHUD;
    [SerializeField]
    GameObject CloseButton;

    // Start is called before the first frame update
    void Start()
    {
        PauseUI.SetActive(false);
        GameHUD.SetActive(false);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowClose()
    {
        CloseButton.SetActive(true); 
    }

    public void CloseInstructionUI()
    {
        this.gameObject.SetActive(false);
        PauseUI.SetActive(true);
        GameHUD.SetActive(true);
        Time.timeScale = 1;
    }
}
