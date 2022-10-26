using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishCount : MonoBehaviour
{
    public TextMeshProUGUI fishCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFishCount(int foundFish)
    {
        fishCount.SetText("Fish " + foundFish + "/100");
    }
}
