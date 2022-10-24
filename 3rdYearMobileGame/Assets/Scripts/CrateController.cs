using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    public int heldFish = 5;

    public GameObject fish;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

   void OnTriggerEnter(Collider collision)
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

    private void OnCollisionEnter(Collision collision)
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
