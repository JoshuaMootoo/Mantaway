using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishCount : MonoBehaviour
{
    public TextMeshProUGUI fishCount;

    public int collectedfish;
    public int maxFish;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFishCount(int foundFish)
    {
        collectedfish = foundFish;
        fishCount.SetText("Fish " + collectedfish + "/" + maxFish);
    }
}
