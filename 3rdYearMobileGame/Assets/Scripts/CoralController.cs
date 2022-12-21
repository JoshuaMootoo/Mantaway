using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralController : MonoBehaviour
{
    float x;
    float y;

    float perlin;
    // Start is called before the first frame update
    void Start()
    {
        x = transform.position.x;
        y = transform.position.z;

        perlin = Mathf.PerlinNoise(x, y);
        Mathf.Clamp01(perlin);

        gameObject.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_BaseColor"), Random.ColorHSV(perlin, perlin, 0.7f, 0.9f, 1f, 1f, 1f, 1f));
        transform.rotation = Quaternion.Euler( new Vector3(0, perlin * 360));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
