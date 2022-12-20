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
    //public bool found = false;
    
    //Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        //rb = GetComponent<Rigidbody>();
        


        gameObject.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_BaseColor"), Random.ColorHSV(0f, 1f, 0.5f, 1f, 1f, 1f, 1f, 1f));
    }

    
    void Update()
    {
       
    }

    public void Movement(Vector3 targetPoint,Vector3 playerPosition, float deltaTime)
    {
       // if ((targetPoint - transform.position).magnitude > 1.5f) fishSpeed *= 2f;
       // if ((targetPoint - transform.position).magnitude < 1f) fishSpeed *= 0.5f;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, fishSpeed * deltaTime);

        //Vector3 lookPoint = ((playerPosition - targetPoint) / 2) + targetPoint;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetPoint - transform.position), 8);
        //transform.Translate(transform.forward * fishSpeed * deltaTime,Space.Self);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPoint - transform.position), 0.15F);
        
       
    }

    public void Dead()
    {
        GetComponent<Collider>().isTrigger = false;

        GetComponent<Rigidbody>().AddForce(-transform.forward, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized * 0.68f, ForceMode.Impulse);

        Destroy(gameObject, 5);

    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().AddForce((transform.position - collision.gameObject.transform.position).normalized * (collision.gameObject.GetComponent<PlayerController>().playerSpeed * 1.5f), ForceMode.Impulse);
            GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized * 0.68f, ForceMode.Impulse);
        }
    }
    
}
