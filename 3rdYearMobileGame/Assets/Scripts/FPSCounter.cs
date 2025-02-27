using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI display_Text;

    public int Granularity = 5; // how many frames to wait until you re-calculate the FPS
    List<double> times;
    int counter = 5;

    public void Start()
    {
        times = new List<double>();
    }

    public void Update()
    {
        if (counter <= 0)
        {
            CalcFPS();
            counter = Granularity;
        }

        times.Add(Time.deltaTime);
        counter--;
    }

    public void CalcFPS()
    {
        double sum = 0;
        foreach (double F in times)
        {
            sum += F;
        }

        double average = sum / times.Count;
        double fps = 1 / average;

        int intFPS = Mathf.RoundToInt((float)fps);

        display_Text.SetText(intFPS.ToString());
    }

    //https://stackoverflow.com/questions/32251805/calculating-the-frame-rate-in-a-unity-scene
    // User MKII
}
