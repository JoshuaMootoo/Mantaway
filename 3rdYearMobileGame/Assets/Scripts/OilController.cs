using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilController : MonoBehaviour
{
    float oilSpeed;
    float oilStarterSpeed;
    public float oilAcceleration = 0.2f;

    public float speedMultiplyer = 1.5f;
    public float startDistanceFromPlayer;
    public float distanceFromPlayer;

    PlayerController player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        oilStarterSpeed = player.playerDefaultSpeed - 1;
        oilSpeed = oilStarterSpeed;
        startDistanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceFromPlayer > startDistanceFromPlayer) transform.Translate(Vector3.forward * oilSpeed * speedMultiplyer * Time.deltaTime);
        else transform.Translate(Vector3.forward * oilSpeed * Time.deltaTime);

        oilSpeed += oilAcceleration * Time.deltaTime;
    }
}
