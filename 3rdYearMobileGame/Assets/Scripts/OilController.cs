using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilController : MonoBehaviour
{
    float oilSpeed;
    float oilStarterSpeed;
    public float oilAcceleration = 0.2f;

    PlayerController player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        oilStarterSpeed = player.playerDefaultSpeed - 1;
        oilSpeed = oilStarterSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * oilSpeed * Time.deltaTime);

        oilSpeed += oilAcceleration * Time.deltaTime;
    }
}
