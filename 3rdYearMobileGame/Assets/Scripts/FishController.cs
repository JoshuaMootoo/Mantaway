using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public bool following = false;
    public float fishSpeed = 5;
    public float startTime;
    public int fishNumber;
    public int fishTeam;
    
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        rb = GetComponent<Rigidbody>();
        
        //Randomise Colour
        gameObject.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_BaseColor"), Random.ColorHSV(0f, 1f, 0.5f, 1f, 1f, 1f, 1f, 1f));
    }

    //Movement function called by the fishmanager when the fish is in a list and following the player. Will  not move itself otherwise
    public void Movement(Vector3 targetPoint,Vector3 playerPosition, float deltaTime)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, fishSpeed * deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetPoint - transform.position), 8);
    }

    public void Dead()
    {
        //enable rigidbody collisions and add small force backwards
        GetComponent<Collider>().isTrigger = false;

        rb.AddForce(-transform.forward, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized * 0.68f, ForceMode.Impulse);

        Destroy(gameObject, 5);

    }


    private void OnCollisionEnter(Collision collision)
    {
        //Get nudged by player when they are dead
        if(collision.gameObject.CompareTag("Player"))
        {
            rb.AddForce((transform.position - collision.gameObject.transform.position).normalized * (collision.gameObject.GetComponent<PlayerController>().playerSpeed * 1.5f), ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized * 0.68f, ForceMode.Impulse);
        }
    }
    
}
