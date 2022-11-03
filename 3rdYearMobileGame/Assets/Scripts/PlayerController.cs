using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Health variables
    public float health;
    public float maxHealth = 3;
    public float healingDelayTimer = 0;
    public float healingDelayTimerMax = 4;
    public float healingRate = 0.5f;
    public bool healing;

   
    public int foundFish;
    
    //Player speed variables
    public float playerSpeed = 5f;
    public float playerDefaultSpeed = 5f;
    public float playerSlowSpeed = 2.5f;
    public float playerBoostSpeed = 10f;

    //Boost variables
    [HideInInspector] float boostCharge;
    public float boostChargeMax = 100;
    public float boostChargeRate = 100;
    [HideInInspector] float boostTime = 3f;
    public float boostTimeMax = 3f;
    public bool boosting = false;
    public bool charging = false;

    public float turnRate = 150f;
    public Transform[] pastPosition;

    Rigidbody rb;
    CharacterController controller;

    AudioManager audioManager;

    //Hud objects
    public FishCount fishCount;
    public HealthBar healthBar;
    public BoostBar boostBar;

    //Trail Renderers
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

        audioManager = FindObjectOfType<AudioManager>();

        // find and set animator IDs
        hasAnimator = TryGetComponent(out animator);
        AssignAnimationIDs();


        rightWing.emitting = false;
        leftWing.emitting = false;

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        hasAnimator = TryGetComponent(out animator);

        Movement();

        boostBar.SetBoostBar(boostCharge / boostChargeMax);
        healthBar.SetHealthBar(health / maxHealth);
        fishCount.SetFishCount(foundFish);

        if (healingDelayTimer > 0)
        {
            healing = false;
            healingDelayTimer -= 1f * Time.deltaTime;
        }

        if  (healingDelayTimer <= 0)
        {
            healing = true;
        }

        if(health < maxHealth & healingDelayTimer <= 0)
        {
            health += healingRate * Time.deltaTime;
        }

        if (health <= 0)
        {
            FindObjectOfType<GameManager>().GameOver();
            Destroy(gameObject);
        }

        touchDebug();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {

            //gameObject.GetComponent<TrailRenderer>().material.SetColor(Shader.PropertyToID("_BaseColor"), collision.gameObject.GetComponent<Renderer>().material.GetColor(Shader.PropertyToID("_BaseColor")));
            //gameObject.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_BaseColor"), collision.gameObject.GetComponent<Renderer>().material.GetColor(Shader.PropertyToID("_BaseColor")));

            if (collision.gameObject.GetComponent<FishController>().following == false)
            {
                collision.gameObject.GetComponent<FishController>().following = true;
                foundFish += 1;

                Debug.Log("Found Fish " + foundFish);
                audioManager.Play("CollectFish");

            }

        }
        
        if(collision.gameObject.CompareTag("Mine"))
        {
            health -= 1;
            healingDelayTimer = healingDelayTimerMax;

            Debug.Log("Health = " + health);
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            audioManager.Play("TerrainCollision");
            Debug.Log("terrain collision");
        }

    }



    //Player Movement and Input
    void Movement()
    {

        playerSpeed = playerDefaultSpeed;

        rightWing.emitting = false;
        leftWing.emitting = false;


        animator.SetBool(animIDTLeft, false);
        animator.SetBool(animIDTRight, false);
        animator.SetBool(animIDCharging, false);
        animator.SetBool(animIDBoost, false);

        //touch Controls
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToViewportPoint(touch.position);
            Debug.Log(touchPosition);
            if (Input.touchCount < 2)
            {
                charging = false;
                if (touchPosition.x < .5)
                {
                    turnLeft();
                }
                if (touchPosition.x >= .5)
                {
                    turnRight();
                }
            }
            if (Input.touchCount >= 2)
            {

                slowDown();
            }

        }

        //Keyboard Controls
        if (Input.GetKey(KeyCode.LeftArrow) & !Input.GetKey(KeyCode.RightArrow))
        {
            turnLeft();
        }

        else if (Input.GetKey(KeyCode.RightArrow) & !Input.GetKey(KeyCode.LeftArrow))
        {
            turnRight();
        }

        else if (Input.GetKey(KeyCode.LeftArrow) & Input.GetKey(KeyCode.RightArrow))
        {
            slowDown();
        }
        else if (!Input.GetKey(KeyCode.LeftArrow) & !Input.GetKey(KeyCode.RightArrow))
        {
            charging = false;
        }


        if (charging & boostCharge < boostChargeMax)
        {
            boostCharge = boostCharge + (boostChargeRate * Time.deltaTime);
            Debug.Log("boostCharge = " + boostCharge);
        }
        else if(!charging & boostCharge > 0  || boosting & boostCharge > 0)
        {
            boostCharge = boostCharge - (boostChargeRate * Time.deltaTime);
            audioManager.Stop("BoostCharging");
        }


        //Initiate Boosting
        if (boostCharge > boostChargeMax & boosting == false)
        {
            boosting = true;
            boostTime = boostTimeMax;
            audioManager.Play("Boosting");
            audioManager.Play("BoostStart");
            audioManager.Stop("BoostCharging");
        }

        if (boosting == true)
        {
            rightWing.emitting = true;
            leftWing.emitting = true;
            //playerSpeed = playerBoostSpeed;
            playerSpeed = Mathf.Lerp(playerBoostSpeed, playerSpeed, Time.deltaTime);
            boostTime = boostTime - (1 * Time.deltaTime);
            Debug.Log("BoostTime = " + boostTime);
            animator.SetBool(animIDBoost, true);

            if (boostTime <= 0) //stop boosting 
            {
                boosting = false;
                audioManager.Stop("Boosting");
            }
        }

        void turnLeft()
        {
            transform.Rotate(0, -150f * Time.deltaTime, 0);

            //boostCharge = 0;
            charging = false;
            rightWing.emitting = true;
            leftWing.emitting = false;

            currentPlayerState = PlayerState.turningLeft;
            animator.SetBool(animIDTLeft, true);
        }

        void turnRight()
        {
            transform.Rotate(0, 150f * Time.deltaTime, 0);

            //boostCharge = 0;
            charging = false;
            rightWing.emitting = false;
            leftWing.emitting = true;

            currentPlayerState = PlayerState.turningRight;
            animator.SetBool(animIDTRight, true);
        }

        void slowDown()
        {
            rightWing.emitting = false;
            leftWing.emitting = false;
            playerSpeed = playerSlowSpeed;

            if(!charging & !boosting)
            {
                charging = true;
                audioManager.Play("BoostCharging");
            }

            currentPlayerState = PlayerState.slowingDown;
            animator.SetBool(animIDCharging, true);
           
        }


        //final Movement of player position

        controller.Move(transform.forward * playerSpeed * Time.deltaTime);

        //rb.MovePosition(transform.position + new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * playerSpeed * Time.deltaTime);

        //transform.position = transform.position + transform.forward * playerSpeed * Time.deltaTime;


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
