using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public bool following = false;
    //public bool found = false;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, 4.0f * Time.deltaTime);
        }
    }

   
}
