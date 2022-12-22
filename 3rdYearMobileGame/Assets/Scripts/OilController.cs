using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilController : MonoBehaviour
{
    [SerializeField] float oilSpeed;
    [SerializeField] float oilStartSpeed;
    public float oilAcceleration = 0.2f;

    public float speedMultiplyer = 1.5f;
    public float startDistanceFromPlayer;
    public float distanceFromPlayer;

    PlayerController player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        oilStartSpeed = player.playerDefaultSpeed - 1.5f;
        oilSpeed = oilStartSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceFromPlayer > startDistanceFromPlayer) transform.Translate(Vector3.forward * oilSpeed * speedMultiplyer * Time.deltaTime);
        else transform.Translate(Vector3.forward * oilSpeed * Time.deltaTime);

        //oilSpeed += oilAcceleration * Time.deltaTime;
    }

    public void OilSpeedUp(int fishFound)
    {
        oilSpeed = oilStartSpeed + (oilAcceleration * fishFound);
    }
}
