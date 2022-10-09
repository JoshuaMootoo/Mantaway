using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;

   // Vector3 cameraPosition;
    // Start is called before the first frame update
    void Start()
    {
      //  cameraPosition = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3( player.position.x, player.position.y, -10);
    }
}
