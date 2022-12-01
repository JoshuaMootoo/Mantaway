using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishCount : MonoBehaviour
{
    public TextMeshProUGUI fishCount;
    public Slider slider;

    public int collectedfish;
    public int maxFish;

    float sliderPercent;

    // Start is called before the first frame update
    void Start()
    {
       foreach( FishController fish in FindObjectsOfType<FishController>())
        {
            maxFish += 1;
        }

       foreach (CrateController crate in FindObjectsOfType<CrateController>())
        {
            maxFish += crate.heldFish;
        }
        slider = GetComponent<Slider>();
        slider.maxValue = maxFish;
    }


    public void SetFishCount(int foundFish)
    {
        collectedfish = foundFish;
        fishCount.SetText(collectedfish + "/" + maxFish);
        slider.value = foundFish;
    }
}
