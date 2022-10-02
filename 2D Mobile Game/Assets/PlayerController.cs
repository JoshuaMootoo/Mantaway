using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Vector2 forward = new Vector2(0, 10);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * 5f * Time.deltaTime, Space.Self);
        }

            if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, 0, 150f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {

           
            transform.Rotate(0, 0, -150f * Time.deltaTime);
        }
    }




    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Circle"))
        {
            // gameObject.GetComponent<TrailRenderer>().material.color = collision.gameObject.GetComponent<Material>().color;

            gameObject.GetComponent<TrailRenderer>().material.SetColor(Shader.PropertyToID("_BaseColor"), collision.gameObject.GetComponent<Renderer>().material.GetColor(Shader.PropertyToID("_BaseColor")));

            //collision.gameObject.GetComponent<CircleController>().colour);
            //collision.gameObject.GetComponent<Material>().shader.GetPropertyAttributes();

            // Debug.Log(collision.gameObject.GetComponent<MeshRenderer>().material.shader.GetPropertyCount());

            // int length = collision.gameObject.GetComponent<MeshRenderer>().material.shader.GetPropertyCount();
            // for (int i = 0; i < length; i++)
            //  Debug.Log(collision.gameObject.GetComponent<MeshRenderer>().material.shader.get);


            Destroy(collision.gameObject);

        }
    }
}
