using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public bool following = false;
    public float fishSpeed = 4;
    public float startTime;
    //public bool found = false;
    
    //Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        //rb = GetComponent<Rigidbody>();
        


        gameObject.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_BaseColor"), Random.ColorHSV());
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (following & (Time.time - startTime) > 0.5f)
    //    {
    //         transform.position = Vector3.MoveTowards(transform.position, player.position, 4.0f * Time.deltaTime);
    //        // rb.AddForce((player.position - transform.position), ForceMode.Acceleration);
    //        //rb.velocity = (player.position - transform.position).normalized * fishSpeed;

    //    }
    //}

    public void Movement(Vector3 targetPoint,float deltaTime)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, fishSpeed * deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPoint - transform.position), 0.15F);
    }

   
}
