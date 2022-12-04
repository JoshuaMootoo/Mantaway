using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionBehaviour : MonoBehaviour
{
    
   
    GameObject GameHUD;
    [SerializeField]
    GameObject CloseButton;
    [SerializeField]
    float slowmotionSpeed = 0.03f; 
    [SerializeField]
    float timerLength = 2;
    float timer;

    void Awake()
    {
        GameHUD = GameObject.FindGameObjectWithTag("HUD");
    }

    // Start is called before the first frame update
    void Start()
    {
        GameHUD.SetActive(false);
        Time.timeScale = slowmotionSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < timerLength & !CloseButton.activeSelf) timer += 1 * Time.unscaledDeltaTime;
        if (timer >= timerLength & !CloseButton.activeSelf)
        {
            ShowClose();
        }
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
