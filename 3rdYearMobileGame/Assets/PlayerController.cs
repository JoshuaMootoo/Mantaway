using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 forward = new Vector2(0, 1);
    public int foundFish;

    public Transform[] pastPosition;

    Rigidbody2D rb;

    public enum PlayerState
    {
        turningLeft,
        turningRight,
        slowingDown,
        speedingUp
    }

    public PlayerState currentPlayerState;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToViewportPoint(touch.position);
            Debug.Log(touchPosition);
            if(Input.touchCount < 2)
            {
                if (touchPosition.x < .5)
                {
                    transform.Rotate(0, 0, 150f * Time.deltaTime);
                    currentPlayerState = PlayerState.turningLeft;
                }
                if (touchPosition.x >= .5)
                {
                    transform.Rotate(0, 0, -150f * Time.deltaTime);
                    currentPlayerState = PlayerState.turningRight;
                }
            }
            if(Input.touchCount >= 2)
            {
                transform.Translate(Vector3.up * -2.5f * Time.deltaTime, Space.Self);
                currentPlayerState = PlayerState.slowingDown;

            }


        }
       // if (Input.GetKey(KeyCode.UpArrow))
        {
             transform.Translate(Vector3.up * 5f * Time.deltaTime, Space.Self);
            //rb.AddForce(forward * 10f);
        }

        if (Input.GetKey(KeyCode.LeftArrow) )
        {
            transform.Rotate(0, 0, 150f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {


            transform.Rotate(0, 0, -150f * Time.deltaTime);
        }

        touchDebug();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {

           // gameObject.GetComponent<TrailRenderer>().material.SetColor(Shader.PropertyToID("_BaseColor"), collision.gameObject.GetComponent<Renderer>().material.GetColor(Shader.PropertyToID("_BaseColor")));

            //gameObject.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_BaseColor"), collision.gameObject.GetComponent<Renderer>().material.GetColor(Shader.PropertyToID("_BaseColor")));

           // Destroy(collision.gameObject);
           if (collision.gameObject.GetComponent<FishController>().following == false)
            foundFish += 1;
            Debug.Log("Found Fish " + foundFish);

        }
    }


    void touchDebug()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Vector3 debugTouchPosition = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
            Debug.DrawLine(transform.position, debugTouchPosition, Color.red);

           
        }
    }
}
