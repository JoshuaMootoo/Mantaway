using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitboxWithEnable : MonoBehaviour
{

    PlayerController player;
    FishManager FishManager;
    AudioManager audioManager;

    bool hasAttacked;

    public float maxKilledFish = 5;
    float killedFish = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        FishManager = FindObjectOfType<FishManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnEnable()
    {
        hasAttacked = false;
    }

    private void OnDisable()
    {
        audioManager.Play("EelBite");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") & !hasAttacked)
        {
            player.health -= 1;
            player.healingDelayTimer = player.healingDelayTimerMax;
            hasAttacked = true;
        }

        if (other.gameObject.CompareTag("Fish") & killedFish < maxKilledFish)
        {
            FishController fish = other.gameObject.GetComponent<FishController>();
            Debug.Log("fish eaten");
            FishManager.EatFish(fish.fishNumber, fish.fishTeam, fish);
            killedFish++;
            if (killedFish == maxKilledFish) Invoke("ResetKilledFish", 5);
        }
        

    }

    void ResetKilledFish()
    {
        killedFish = 0;
    }
}
