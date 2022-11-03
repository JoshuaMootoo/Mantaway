using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject explosionParticleEmitter;

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

           if(explosionParticleEmitter != null) Instantiate(explosionParticleEmitter, transform.position, transform.rotation);

            FindObjectOfType<AudioManager>().Play("MineExplosion");

            Destroy(gameObject);    
        }
    }
}
