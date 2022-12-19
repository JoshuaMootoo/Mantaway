using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    public int heldFish = 5;
    public int fishRequirement = 0;
    public float torqueStrength = 50;

    public GameObject crateParticleEmitter;
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
                rb.AddForce((transform.position - collision.gameObject.transform.position).normalized * player.GetComponent<PlayerController>().playerSpeed, ForceMode.Impulse);
                rb.AddTorque(new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized * torqueStrength, ForceMode.Impulse);

                FindObjectOfType<AudioManager>().Play("CrateCollision");
            }

            //Break Crate when colliding with it during boost
            if (collision.gameObject.GetComponent<PlayerController>().boosting == true & player.GetComponent<PlayerController>().foundFish >= fishRequirement)
            {
                for(int i = 0; i < heldFish; i++)
                {
                    fishManager.SpawnFish(1, transform.position + new Vector3(Random.Range(-1,1) ,0,Random.Range(-1,1)));
                }

                //Handheld.Vibrate();

                FindObjectOfType<AudioManager>().Play("CrateSmash");

                //Spawn Particles
                if (crateParticleEmitter != null) Instantiate(crateParticleEmitter, transform.position, transform.rotation);

                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Break Crate when colliding with it during boost
            if (collision.gameObject.GetComponent<PlayerController>().boosting == true & player.GetComponent<PlayerController>().foundFish >= fishRequirement)
            {
                for (int i = 0; i < heldFish; i++)
                {
                    fishManager.SpawnFish(1, transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)));
                }


                FindObjectOfType<AudioManager>().Play("CrateSmash");

                //Spawn Particles
                if (crateParticleEmitter != null) Instantiate(crateParticleEmitter, transform.position, transform.rotation);

                Destroy(gameObject);
            }
        }
    }
}
