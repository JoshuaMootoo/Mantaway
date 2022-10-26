using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    public int foundFish;
    public float playerSpeed = 5f;
    public float playerDefaultSpeed = 5f;
    public float playerSlowSpeed = 2.5f;
    public float playerBoostSpeed = 10f;
    public float boostCharge;
    public float boostTime = 3f;

    public bool boosting = false;

    public float turnRate = 150f;
    public Transform[] pastPosition;

    Rigidbody rb;
    CharacterController controller;

    public TrailRenderer rightWing;
    public TrailRenderer leftWing;

    // Animator
    private bool hasAnimator;
    private Animator animator;

    // Animation IDs
    private int animIDSpeed;
    private int animIDBoost;
    private int animIDCharging;
    private int animIDTLeft;
    private int animIDTRight;


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
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        // find and set animator IDs
        hasAnimator = TryGetComponent(out animator);
        AssignAnimationIDs();

        rightWing.emitting = false;
        leftWing.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        hasAnimator = TryGetComponent(out animator);


        Vector3 turnRight = new Vector3(0, 1, 0);
        Vector3 turnLeft = new Vector3(0, -1, 0);

        playerSpeed = playerDefaultSpeed;

        rightWing.emitting = false;
        leftWing.emitting = false;
        animator.SetBool(animIDTLeft, false);
        animator.SetBool(animIDTRight, false);
        animator.SetBool(animIDCharging, false);
        animator.SetBool(animIDBoost, false);


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToViewportPoint(touch.position);
            Debug.Log(touchPosition);
            if(Input.touchCount < 2)
            {
                if (touchPosition.x < .5)
                {
                    transform.Rotate(new Vector3(0, 1,0) * turnRate  * Time.deltaTime);
                    currentPlayerState = PlayerState.turningLeft;
                    animator.SetBool(animIDTLeft, true);
                }
                if (touchPosition.x >= .5)
                {
                    transform.Rotate(new Vector3(0, -1, 0)* turnRate * Time.deltaTime);
                    currentPlayerState = PlayerState.turningRight;
                    animator.SetBool(animIDTRight, true);
                }
            }
            if(Input.touchCount >= 2)
            {
                transform.Translate(Vector3.forward * -2.5f * Time.deltaTime, Space.Self);
                currentPlayerState = PlayerState.slowingDown;

            }

        }

         if (Input.GetKey(KeyCode.LeftArrow) & !Input.GetKey(KeyCode.RightArrow))
        {
            //transform.Rotate( new Vector3( 0, -1, 0) * turnRate * Time.deltaTime);
            transform.Rotate(0, -150f * Time.deltaTime, 0);
            boostCharge = 0;
            rightWing.emitting = true;
            leftWing.emitting = false;
            animator.SetBool(animIDTLeft, true);
        }

        else if (Input.GetKey(KeyCode.RightArrow) & !Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.Rotate(turnRight * turnRate * Time.deltaTime);
            transform.Rotate(0, 150f * Time.deltaTime, 0);
            boostCharge = 0;
            rightWing.emitting = false;
            leftWing.emitting = true;
            animator.SetBool(animIDTRight, true);
        }

        else if (Input.GetKey(KeyCode.LeftArrow) & Input.GetKey(KeyCode.RightArrow))
        {
            rightWing.emitting = false;
            leftWing.emitting = false;
            playerSpeed = playerSlowSpeed;
            boostCharge = boostCharge + (75 * Time.deltaTime);
            Debug.Log("boostCharge = " + boostCharge);
            animator.SetBool(animIDCharging, true);
        }

         if (boostCharge > 100 & boosting == false)
        {
            boosting = true;
            boostTime = 3;
           // playerSpeed = playerBoostSpeed;
        }

        if (boosting == true)
        {
            rightWing.emitting = true;
            leftWing.emitting = true;
            playerSpeed = playerBoostSpeed;
            boostTime = boostTime - (1 * Time.deltaTime);
            Debug.Log("BoostTime = " + boostTime);
            if (boostTime <= 0) boosting = false;
            animator.SetBool(animIDBoost, true);
        }


         controller.Move(transform.forward * playerSpeed * Time.deltaTime);





        touchDebug();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {

            // gameObject.GetComponent<TrailRenderer>().material.SetColor(Shader.PropertyToID("_BaseColor"), collision.gameObject.GetComponent<Renderer>().material.GetColor(Shader.PropertyToID("_BaseColor")));

            //gameObject.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_BaseColor"), collision.gameObject.GetComponent<Renderer>().material.GetColor(Shader.PropertyToID("_BaseColor")));

            // Destroy(collision.gameObject);
            if (collision.gameObject.GetComponent<FishController>().following == false)
            {
                collision.gameObject.GetComponent<FishController>().following = true;
                foundFish += 1;
                //collision.gameObject.GetComponent<FishController>().following = true;
            }
           Debug.Log("Found Fish " + foundFish);

        }
    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDBoost = Animator.StringToHash("Boosting");
        animIDCharging = Animator.StringToHash("Charging");
        animIDTLeft = Animator.StringToHash("TurnLeft");
        animIDTRight = Animator.StringToHash("TurnRight");
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
