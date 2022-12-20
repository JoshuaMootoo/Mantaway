using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitboxWithEnable : MonoBehaviour
{

    PlayerController player;
    FishManager FishManager;

    bool hasAttacked;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        FishManager = FindObjectOfType<FishManager>();
    }

    private void OnEnable()
    {
        hasAttacked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") & !hasAttacked)
        {
            player.health -= 1;
            player.healingDelayTimer = player.healingDelayTimerMax;
            hasAttacked = true;
        }

        if (other.gameObject.CompareTag("Fish"))
        {
            FishController fish = other.gameObject.GetComponent<FishController>();
            Debug.Log("fish eaten");
            FishManager.EatFish(fish.fishNumber, fish.fishTeam, fish);
            
        }

    }
}
