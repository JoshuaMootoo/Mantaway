using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_BaseColor"), Random.ColorHSV(0f, 1f, 0.7f, 0.9f, 1f, 1f, 1f, 1f));
        transform.rotation = Quaternion.Euler( new Vector3(0, Random.Range(0, 360)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
