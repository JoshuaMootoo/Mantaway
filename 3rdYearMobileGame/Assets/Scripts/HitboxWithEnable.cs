using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitboxWithEnable : MonoBehaviour
{

    PlayerController player;

    bool hasAttacked;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void OnEnable()
    {
        hasAttacked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!hasAttacked)
        {
            player.health -= 1;
            player.healingDelayTimer = player.healingDelayTimerMax;
            hasAttacked = true;
        }
    }
}
