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
        Invoke("ShowClose", 5);
        PauseUI.SetActive(false);
        GameHUD.SetActive(false); 
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
    }
}
