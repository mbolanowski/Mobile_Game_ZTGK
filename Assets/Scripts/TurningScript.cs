using System.Collections;
using System.Drawing;
using UnityEngine;

public class TurningScript : MonoBehaviour
{
    [Header("Touch area")]
    public int XMargin;
    public int YMargin;
    public int BoxHeight;

    [Header("Movement")]
    public int StartSpeedLevel;
    public float[] SpeedLevels;
    public float[] SideSpeedLevels;
    public float MaxLeanAngle;
    public float MaxSideSpeed;
    //public float FlySpeed;
    public float rotationSpeed;
    //public float speedIncrease = 0.6f;
    //public float actualFlySpeed;
    public float SpeedDecreaseTime;
    private float currentTime = 0;

    [Header("Screen Shake")]
    public float screenShakeDuration = 1.0f;
    public float screenShakeAmount = 1.0f;
    private float screenShakeTimer = 0;
    private bool isShaking = false;

    [Header("Boost")]
    public float BoostIncrease;
    public float BoostDurationIncrease;
    public float BoostDurationPeak;
    public float BoostDurationDecrease;
    private bool isBoosting = false;

    [Header("Score")]
    public float baseDistanceScoreMultiplier = 1.0f;
    public float multiplierIncrease = 1.0f;

    [Header("FoV change")]
    public float[] SpeedLevelsFOVs;
    public float BoostFOVIncrease;

    [Header("Others")]
    public Transform birdRotationTransform;
    public Camera playerCamera;
    //public float boostFOVIncrease = 20f; // How much to increase FOV during boost
    //public float boostLerpSpeed = 10f; // Speed of the initial burst lerp
    public bool isDead = false;
    public ParticleSystem pickupParticles;

    private const float minError = 0.01f;

    private bool leanChanged = false;
    private Rigidbody rb;
    private Vector2 targetPos;
    private Vector2 defualtPos;
    private float halfXpos;
    private float xMult;
    private Rectangle Rectangle;
    private Vector3 baseRotation;

    private float targetLeanPer;
    private float currentLeanPer = 0.0f;

    // Boost variables
    
    //private float boostSpeedMultiplier = 2.0f; // Multiplier for the burst of speed
    //private float boostDuration = 0.5f; // Duration of the burst
    //private float boostCooldown = 2.0f; // Cooldown before returning to normal
    
    //private float baseFlySpeed; // This will be our permanent base speed that increases
    //private float originalFOV; // Store the original camera FOV

    public static int CurrentScore = 0;

    
    private float targetSpeed;

    [Header("Temp")]
    public float currentSpeed;
    public float currentFOV;
    public static float currentScoreMultiplier = 1.0f;
    [SerializeField]
    private int CurrentSpeedLevel;

    private Coroutine currentBoost;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        Rectangle = new Rectangle(XMargin, YMargin, Screen.width - 2 * XMargin, BoxHeight);
        halfXpos = (float)Rectangle.Width * 0.5f;
        xMult = 2.0f / (float)Rectangle.Width;
        baseRotation = birdRotationTransform.localEulerAngles;
        defualtPos = new Vector2(halfXpos, 0);

        CurrentSpeedLevel = StartSpeedLevel < SpeedLevels.Length ? StartSpeedLevel : SpeedLevels.Length - 1;
        currentSpeed = SpeedLevels[CurrentSpeedLevel];

        if(playerCamera != null) currentFOV = playerCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        HandleTouchInput();
    }
    IEnumerator ScreenShake()
    {
        Vector3 defaultPositon = playerCamera.transform.localPosition;
        isShaking = true;
        while (screenShakeTimer < screenShakeDuration)
        {
            playerCamera.transform.localPosition = defaultPositon + Random.insideUnitSphere * screenShakeAmount;
            screenShakeTimer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        screenShakeTimer = 0;
        playerCamera.transform.localPosition = defaultPositon;
        isShaking = false;
    }

    public void RunScreenShake()
    {
        if (isShaking)
        {
            screenShakeTimer = 0;
        } 
        else
        {
            StartCoroutine("ScreenShake");
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        if (leanChanged) UpdateLean();
        Move();
        UpdateScore();
    }

    private void UpdateScore()
    {
        CurrentScore += (int)(currentSpeed * Time.fixedDeltaTime * baseDistanceScoreMultiplier * currentScoreMultiplier);
    }

    private void Move()
    {
        Vector3 currentPos = transform.position;
        currentPos.x -= MaxSideSpeed * Time.fixedDeltaTime * currentLeanPer;
        currentPos.z -= currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(currentPos);
        //rb.MovePosition(new Vector3(0f,0f,0f));
        if(SpeedDecreaseTime < currentTime)
        {
            if(!TookHit())GameManager.Instance.TriggerGameOver();
        }
        currentTime += Time.fixedDeltaTime;
    }

    private void UpdateLean()
    {
        if (!Rectangle.Contains((int)targetPos.x, (int)targetPos.y))
        {
            targetPos = defualtPos;
            leanChanged = true;
        }

        targetLeanPer = (targetPos.x - halfXpos) * xMult;

        float rotationSpeedPer = rotationSpeed / MaxLeanAngle;

        float tempAngle;
        if (targetLeanPer - currentLeanPer < 0)
        {
            tempAngle = currentLeanPer + rotationSpeedPer * Time.fixedDeltaTime * -1.0f;
            tempAngle = Mathf.Max(tempAngle, targetLeanPer);
        }
        else
        {
            tempAngle = currentLeanPer + rotationSpeedPer * Time.fixedDeltaTime * 1.0f;
            tempAngle = Mathf.Min(tempAngle, targetLeanPer);
        }

        currentLeanPer = tempAngle;
        if (Mathf.Abs(currentLeanPer - targetLeanPer) <= minError)
        {
            leanChanged = false;
            currentLeanPer = targetLeanPer;
        }

        baseRotation.z = currentLeanPer * MaxLeanAngle;
        birdRotationTransform.eulerAngles = baseRotation;
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                targetPos = touch.position;
                leanChanged = true;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                targetPos = touch.position;
                leanChanged = true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                targetPos = defualtPos;
                leanChanged = true;
            }
        }
    }

    IEnumerator Boost()
    {
        isBoosting = true;
        float speedDif1 = (targetSpeed - currentSpeed)/BoostDurationIncrease;
        float FOVdif1 = ((SpeedLevelsFOVs[CurrentSpeedLevel] + BoostFOVIncrease) - currentFOV) / BoostDurationIncrease;
        while(currentSpeed < targetSpeed)
        {
            currentSpeed = Mathf.Min(currentSpeed + speedDif1 * Time.fixedDeltaTime, targetSpeed);
            currentFOV = Mathf.Min(currentFOV + FOVdif1 * Time.fixedDeltaTime, (SpeedLevelsFOVs[CurrentSpeedLevel] + BoostFOVIncrease));
            playerCamera.fieldOfView = currentFOV;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(BoostDurationPeak);
        float finalTarget = SpeedLevels[CurrentSpeedLevel];
        float speedDif2 = (currentSpeed - finalTarget)/BoostDurationDecrease;
        float FOVdif2 = (currentFOV - SpeedLevelsFOVs[CurrentSpeedLevel]) / BoostDurationDecrease;
        while (currentSpeed > finalTarget)
        {
            currentSpeed = Mathf.Max(currentSpeed - speedDif2 * Time.fixedDeltaTime, finalTarget);
            currentFOV = Mathf.Max(currentFOV - FOVdif2 * Time.fixedDeltaTime, SpeedLevelsFOVs[CurrentSpeedLevel]);
            playerCamera.fieldOfView = currentFOV;
            yield return new WaitForFixedUpdate();
        }
        isBoosting = false;
    }

    public bool TookHit()
    {
        if(CurrentSpeedLevel == 0) return false;
        if (isBoosting)
        {
            StopCoroutine(currentBoost);
            isBoosting=false;
        }
        CurrentSpeedLevel--;
        currentScoreMultiplier = 1.0f;
        currentSpeed = SpeedLevels[CurrentSpeedLevel];
        currentFOV = SpeedLevelsFOVs[CurrentSpeedLevel];
        playerCamera.fieldOfView = currentFOV;
        MaxSideSpeed = SideSpeedLevels[CurrentSpeedLevel];
        currentTime = 0;
        return true;
    }

    public void BoostSpeed()
    {
        currentTime = 0;
        if(CurrentSpeedLevel < SpeedLevels.Length - 1)
        {
            CurrentSpeedLevel++;
        } 
        else
        {
            currentScoreMultiplier += multiplierIncrease;
        }
        targetSpeed = SpeedLevels[CurrentSpeedLevel] + BoostIncrease;

        if (isBoosting)
        {
            StopCoroutine(currentBoost);
            currentBoost = StartCoroutine("Boost");
        } 
        else
        {
            currentBoost = StartCoroutine("Boost");
        }
        MaxSideSpeed = SideSpeedLevels[CurrentSpeedLevel];
        pickupParticles.Play();
    }
}