using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    public int heldFish = 5;
    public float torqueStrength = 50;

    public GameObject fish;
    FishManager fishManager;
    GameObject player;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>().gameObject;
        fishManager = FindObjectOfType<FishManager>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Push Crate ahead of player when colliding with it slowly
            if (collision.gameObject.GetComponent<PlayerController>().boosting == false)
            {
                rb.AddForce((transform.position - collision.gameObject.transform.position).normalized * 10, ForceMode.Impulse);
                rb.AddTorque(new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized * torqueStrength, ForceMode.Impulse);
            }

            //Break Crate when colliding with it during boost
            if (collision.gameObject.GetComponent<PlayerController>().boosting == true)
            {

                fishManager.SpawnFish(heldFish, transform.position);

                Destroy(gameObject);

            }
        }
    }

}
