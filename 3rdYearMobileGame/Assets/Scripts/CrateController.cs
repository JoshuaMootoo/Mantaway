using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    public int heldFish = 5;
    public float torqueStrength = 50;

    public GameObject fish;
    GameObject player;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>().gameObject;
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

                for (int i = 0; i < heldFish; i++)
                {
                   GameObject InstantiatedFish = Instantiate(fish, transform.position, transform.rotation);

                    InstantiatedFish.GetComponent<FishController>().following = true;
                    InstantiatedFish.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1,1), 0, Random.Range(-1,1)).normalized * 5, ForceMode.Impulse);
                    player.gameObject.GetComponent<PlayerController>().foundFish += 1;
                }

                Destroy(gameObject);

            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().boosting == true)
            {

                for (int i = 0; i < heldFish; i++)
                {
                    Instantiate(fish, transform.position, transform.rotation);

                }

                Destroy(gameObject);

            }
        }
    }
}
