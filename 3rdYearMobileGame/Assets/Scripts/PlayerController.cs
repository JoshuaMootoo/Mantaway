using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    public int fishLostFromMine = 3;
    
    //Player speed variables
    public float playerSpeed = 5f;
    public float playerDefaultSpeed = 5f;
    public float playerSlowSpeed = 0.5f;
    public float playerBoostSpeed = 2f;
    float boostLerpTime;
    public float boostLerpTimeSpeed = 3;
    public float addedFishSpeedModifier = 0.1f;
    float slowDownTime = 0;
    float neutralTime = 1;

    //Player Rotation Variables
    public float turnRate = 150f;
    public float rotationRange = 85;
    float rotationY;
    bool correcting = false;
    float initialAngle;
    float correctingTime;

    //Mechanics Triggers
    public bool courseCorrectionEnabled = true;
    public bool angleLockEnabled = true;
    public bool steeringInputFlipped = false;

    //Boost variables
    [HideInInspector] float boostCharge;
    public float boostChargeMax = 100;
    public float boostChargeRate = 100;
    [HideInInspector] float boostTime = 3f;
    public float boostTimeMax = 3f;
    public bool boosting = false;
    public bool charging = false;

    
    public List<Vector3> pastPositionList;
    public List<Vector3> pastPositionList2;
    public List<Vector3> pastPositionList3;

    public List<float> speedList;
    public List<float> speedList2;
    public List<float> speedList3;

    Rigidbody rb;
    CharacterController controller;

    AudioManager audioManager;
    FishManager fishManager;

    //Hud objects
    public UIManager uIManager;
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
        uIManager = FindObjectOfType<UIManager>();

        // find and set animator IDs
        hasAnimator = TryGetComponent(out animator);
        AssignAnimationIDs();

        pastPositionList = new List<Vector3>();
        pastPositionList2 = new List<Vector3>();
        pastPositionList3 = new List<Vector3>();

        speedList = new List<float>();
        speedList2 = new List<float>();
        speedList3 = new List<float>();


        InvokeRepeating("AddToPositionList", 0.1f, 0.2f);
        InvokeRepeating("AddToPositionList2", 0.166f, 0.2f);
        InvokeRepeating("AddToPositionList3", 0.233f, 0.2f);
        rightWing.emitting = false;
        leftWing.emitting = false;

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        hasAnimator = TryGetComponent(out animator);

        Movement();

        if(boostBar != null)boostBar.SetBoostBar(boostCharge / boostChargeMax);
        if(healthBar != null) healthBar.SetHealthBar(health / maxHealth);

        //Volume vignette
        float healthPercent = 1 - (health / maxHealth);
        float vignetteIntensityTarget = 1- ((1 - healthPercent) * (1 - healthPercent));
        FindObjectOfType<Volume>().sharedProfile.TryGet<Vignette>(out var vignette);
        if(vignette.intensity.value != vignetteIntensityTarget)
        {
            vignette.intensity.value += (vignetteIntensityTarget - vignette.intensity.value) * 0.5f;
        }
       

        uIManager.SetFishCount(foundFish);

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
            //Player Death now disabled
            //FindObjectOfType<GameManager>().EndGame(true, false);
            //Destroy(gameObject);
            
            health = 0;
        }

        //touchDebug();
    }

    //Collision Events
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
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

            
            for(int i = 0; i < fishLostFromMine; i++)
            {
                if (foundFish > 0)
                {
                    foundFish -= 1;
                    fishManager.RemoveFromList();
                }
                    
            }
            

            Debug.Log("Health = " + health);

            //Handheld.Vibrate();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Oil"))
        {
            health -= oilDamageRate * Time.deltaTime;
            healingDelayTimer = healingDelayTimerMax;
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

        //Check if device running this is a phone
        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = Camera.main.ScreenToViewportPoint(touch.position);
                Debug.Log(touchPosition);
                if (Input.touchCount < 2)
                {
                    if (touchPosition.x < .5)
                    {
                        if (steeringInputFlipped) turnRight();
                        else turnLeft();
                        correcting = false;
                    }
                    if (touchPosition.x >= .5)
                    {
                        if (steeringInputFlipped) turnLeft();
                        else turnRight();
                        correcting = false;
                    }
                }
                if (Input.touchCount >= 2)
                {
                    slowDown();
                    correcting = false;
                }
            }
            else if (Input.touchCount == 0)
            {
                if (courseCorrectionEnabled)
                {
                    if (!correcting)
                    {
                        initialAngle = rotationY;
                        correctingTime = 0;
                        correcting = true;
                    }
                    if (correcting)
                    {
                        courseCorrection(initialAngle, correctingTime);
                        correctingTime += Time.deltaTime;
                    }
                }
                charging = false;
            }
        }


        //Check if the device running this is a desktop
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //Keyboard Controls
            if (Input.GetKey(KeyCode.LeftArrow) & !Input.GetKey(KeyCode.RightArrow))//Going Left
            {
                if (steeringInputFlipped) turnRight();
                else turnLeft();
                correcting = false;
            }

            else if (Input.GetKey(KeyCode.RightArrow) & !Input.GetKey(KeyCode.LeftArrow))//Going Right
            {
                if (steeringInputFlipped) turnLeft();
                else turnRight();
                correcting = false;

            }

            else if (Input.GetKey(KeyCode.LeftArrow) & Input.GetKey(KeyCode.RightArrow))//Charging
            {
                slowDown();
                correcting = false;
            }
            else if (!Input.GetKey(KeyCode.LeftArrow) & !Input.GetKey(KeyCode.RightArrow))//Neutral
            {
                if (courseCorrectionEnabled)
                {
                    if (!correcting)
                    {
                        initialAngle = rotationY;
                        correctingTime = 0;
                        correcting = true;
                    }
                    if (correcting)
                    {
                        courseCorrection(initialAngle, correctingTime);
                        correctingTime += Time.deltaTime;
                    }
                }
                charging = false;
            }
        }
        


        if (charging & boostCharge < boostChargeMax)
        {
            boostCharge = boostCharge + (boostChargeRate * Time.deltaTime);
            Debug.Log("boostCharge = " + boostCharge);

            float slowDownSmooth = slowDownTime * slowDownTime;

            float slowSpeedTarget = (playerDefaultSpeed + addedFishSpeed) * playerSlowSpeed;
            playerSpeed = (playerDefaultSpeed + addedFishSpeed) * (1 - (playerSlowSpeed * slowDownSmooth));

            slowDownTime += Time.deltaTime * 2;
            Mathf.Clamp01(slowDownTime);
        }
        else if(!charging & boostCharge > 0  || boosting & boostCharge > 0)
        {
            boostCharge = boostCharge - (boostChargeRate * Time.deltaTime);
            audioManager.Stop("BoostCharging");
        }
        if (!charging) slowDownTime = 0;


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
                neutralTime = 0;
            }
        }

        if (!boosting & !charging)
        {
            neutralTime += Time.deltaTime;
            //Mathf.Clamp(neutralTime, 0, 1);
            if (neutralTime > 1) neutralTime = 1;
            playerSpeed += ((playerDefaultSpeed + addedFishSpeed) - playerSpeed) * 0.3f;


            float neutralSmooth = 1 - ((1 - neutralTime) * (1 - neutralTime) * (1 - neutralTime) * (1 - neutralTime));

            float neutralSpeedTarget = (playerDefaultSpeed + addedFishSpeed);
            playerSpeed = (playerDefaultSpeed + addedFishSpeed) * (playerBoostSpeed -  ((playerBoostSpeed - 1) * neutralSmooth));

            
        }
        
        //Final Movement of player position
        if (angleLockEnabled) rotationY = Mathf.Clamp(rotationY, -rotationRange, rotationRange);
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
        controller.Move(transform.forward * playerSpeed * Time.deltaTime);

        //Player Height Correction
        controller.Move((Vector3.up * 2) - (Vector3.up * transform.position.y));



        //PlayerMovement Functions
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


            if (!charging & !boosting)
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
    }
    private void AddToPositionList()
    {
        pastPositionList.Insert(0, transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-2, 2)));
        if (pastPositionList.Count > 100) pastPositionList.RemoveAt(pastPositionList.Count - 1);

        speedList.Insert(0, playerSpeed);
        if (speedList.Count > 100) speedList.RemoveAt(speedList.Count - 1);
    }
    private void AddToPositionList2()
    {
        pastPositionList2.Insert(0, transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-2, 2)));
        if (pastPositionList2.Count > 100) pastPositionList2.RemoveAt(pastPositionList.Count - 1);

        speedList2.Insert(0, playerSpeed);
        if (speedList2.Count > 100) speedList2.RemoveAt(speedList2.Count - 1);

    }
    private void AddToPositionList3()
    {
        pastPositionList3.Insert(0, transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-2, 2)));
        if (pastPositionList3.Count > 100) pastPositionList3.RemoveAt(pastPositionList.Count - 1);

        speedList3.Insert(0, playerSpeed);
        if (speedList3.Count > 100) speedList3.RemoveAt(speedList3.Count - 1);
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
