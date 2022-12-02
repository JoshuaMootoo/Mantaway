using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionBehaviour : MonoBehaviour
{
    
    [SerializeField]
    GameObject GameHUD;
    [SerializeField]
    GameObject CloseButton;

    // Start is called before the first frame update
    void Start()
    {
        GameHUD.SetActive(false);
        Time.timeScale = 0.01f;
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
        GameHUD.SetActive(true);
        Time.timeScale = 1;
    }
}
