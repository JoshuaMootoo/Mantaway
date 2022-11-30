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

    public float oilDamageRate = 1;
    public int foundFish;
    
    //Player speed variables
    public float playerSpeed = 5f;
    public float playerDefaultSpeed = 5f;
    public float playerSlowSpeed = 0.5f;
    public float playerBoostSpeed = 2f;
    float boostLerpTime;
    public float boostLerpTimeSpeed = 3;
    public float addedFishSpeedModifier = 0.1f;

    //Player Rotation Variables
    float rotationY;
    bool correcting = false;
    float initialAngle;
    float correctingTime;

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
    public List<Vector3> pastPositionList;

    Rigidbody rb;
    CharacterController controller;

    AudioManager audioManager;
    FishManager fishManager;

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
        fishManager = FindObjectOfType<FishManager>();

        // find and set animator IDs
        hasAnimator = TryGetComponent(out animator);
        AssignAnimationIDs();

        pastPositionList = new List<Vector3>();

        InvokeRepeating("AddToPositionList", 0.1f, 0.2f);
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
            FindObjectOfType<GameManager>().EndGame(true, false);
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
                FindObjectOfType<FishManager>().AddToList(collision.GetComponent<FishController>());
                Debug.Log("Found Fish " + foundFish);
                audioManager.Play("CollectFish");

            }

        }
        
        if(collision.gameObject.CompareTag("Mine"))
        {
            health -= 1;
            healingDelayTimer = healingDelayTimerMax;
            foundFish -= 1;
            fishManager.RemoveFromList();

            Debug.Log("Health = " + health);
        }

        

        
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Oil"))
        {
            health -= oilDamageRate * Time.deltaTime;
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
        float addedFishSpeed = addedFishSpeedModifier * foundFish;
        playerSpeed = playerDefaultSpeed + addedFishSpeed;

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
            else if (Input.touchCount != 2)
            {
                charging = false;
            }

        }
        //Check if the device running this is a desktop
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //Keyboard Controls
            if (Input.GetKey(KeyCode.LeftArrow) & !Input.GetKey(KeyCode.RightArrow))
            {
                turnLeft();
                correcting = false;

            }

            else if (Input.GetKey(KeyCode.RightArrow) & !Input.GetKey(KeyCode.LeftArrow))
            {
                turnRight();
                correcting = false;

            }

            else if (Input.GetKey(KeyCode.LeftArrow) & Input.GetKey(KeyCode.RightArrow))
            {
                slowDown();
                correcting = false;
            }
            else if (!Input.GetKey(KeyCode.LeftArrow) & !Input.GetKey(KeyCode.RightArrow))
            {
                if(!correcting)
                {
                    initialAngle = rotationY;
                    correctingTime = 0;
                    correcting = true;
                }
                if(correcting)
                {
                    courseCorrection(initialAngle, correctingTime);
                    correctingTime += Time.deltaTime;

                }
                charging = false;
            }
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
            boostLerpTime = 0;
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
      
            playerSpeed = (playerDefaultSpeed  + addedFishSpeed) * Mathf.SmoothStep(playerSlowSpeed, playerBoostSpeed, boostLerpTime);
            boostLerpTime += Time.deltaTime * boostLerpTimeSpeed;

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
            //transform.Rotate(0, -150f * Time.deltaTime, 0);
            rotationY += -150f * Time.deltaTime;

            //boostCharge = 0;
            charging = false;
            rightWing.emitting = true;
            leftWing.emitting = false;

            currentPlayerState = PlayerState.turningLeft;
            animator.SetBool(animIDTLeft, true);
        }

        void turnRight()
        {
            //transform.Rotate(0, 150f * Time.deltaTime, 0);
            rotationY += 150f * Time.deltaTime;

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
            playerSpeed = (playerDefaultSpeed + addedFishSpeed) * playerSlowSpeed;

            if(!charging & !boosting)
            {
                charging = true;
                audioManager.Play("BoostCharging");
            }

            currentPlayerState = PlayerState.slowingDown;
            animator.SetBool(animIDCharging, true);
           
        }

        void courseCorrection(float angle, float timeCount)
        {
            rotationY = Mathf.Lerp(angle, 0, timeCount);
        }



        //final Movement of player position

        rotationY = Mathf.Clamp(rotationY, -90, 90);
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
        controller.Move(transform.forward * playerSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

    }

    private void AddToPositionList()
    {
        pastPositionList.Insert(0, transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-2, 2)));
        if (pastPositionList.Count > 100) pastPositionList.RemoveAt(pastPositionList.Count - 1);
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
