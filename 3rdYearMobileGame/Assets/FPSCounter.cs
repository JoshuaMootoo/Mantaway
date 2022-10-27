using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{


    public int avgFrameRate;
    public TextMeshProUGUI display_Text;

    // Start is called before the first frame update
    void Start()
    {
         InvokeRepeating("GetFPS", 0.1f, 0.1f);

        
    }

    void GetFPS()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        display_Text.SetText(avgFrameRate.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        //GetFPS();
    }
}
