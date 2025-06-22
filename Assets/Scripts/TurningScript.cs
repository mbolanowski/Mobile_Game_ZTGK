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
    public float MaxLeanAngle;
    public float MaxSideSpeed;
    public float FlySpeed;
    public float rotationSpeed;
    public float speedIncrease = 0.6f;
    public float actualFlySpeed;

    [Header("Screen Shake")]
    public float screenShakeDuration = 1.0f;
    public float screenShakeAmount = 1.0f;
    private float screenShakeTimer = 0;
    private bool isShaking = false;

    [Header("Boost")]
    public Transform birdRotationTransform;
    public Camera playerCamera;
    public float boostFOVIncrease = 20f; // How much to increase FOV during boost
    public float boostLerpSpeed = 10f; // Speed of the initial burst lerp

    [Header("Others")]
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
    private bool isBoosting = false;
    private float boostSpeedMultiplier = 2.0f; // Multiplier for the burst of speed
    private float boostDuration = 0.5f; // Duration of the burst
    private float boostCooldown = 2.0f; // Cooldown before returning to normal
    private float boostTimer = 0.0f;
    private float baseFlySpeed; // This will be our permanent base speed that increases
    private float originalFOV; // Store the original camera FOV

    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Rectangle = new Rectangle(XMargin, YMargin, Screen.width - 2 * XMargin, BoxHeight);
        halfXpos = (float)Rectangle.Width * 0.5f;
        xMult = 2.0f / (float)Rectangle.Width;
        baseRotation = birdRotationTransform.localEulerAngles;
        defualtPos = new Vector2(halfXpos, 0);

        baseFlySpeed = FlySpeed;

        if (playerCamera != null)
        {
            originalFOV = playerCamera.fieldOfView;
        }
        actualFlySpeed = FlySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        HandleTouchInput();

        if (isBoosting)
        {
            boostTimer += Time.deltaTime;

            if (boostTimer < boostDuration)
            {
                float targetSpeed = baseFlySpeed * boostSpeedMultiplier;
                FlySpeed = Mathf.Lerp(FlySpeed, targetSpeed, boostLerpSpeed * Time.deltaTime);

                if (playerCamera != null)
                {
                    float targetFOV = originalFOV + boostFOVIncrease;
                    playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, boostLerpSpeed * Time.deltaTime);
                }
            }
            else if (boostTimer < boostDuration + boostCooldown)
            {
                FlySpeed = Mathf.Lerp(FlySpeed, baseFlySpeed * 1.2f, Time.deltaTime);

                // Gradually return FOV to normal
                if (playerCamera != null)
                {
                    float targetFOV = originalFOV;
                    playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV - 1f, Time.deltaTime);
                }
            }
            else
            {
                FlySpeed = baseFlySpeed * 1.2f;

                if (playerCamera != null)
                {
                    playerCamera.fieldOfView = originalFOV;
                }

                isBoosting = false;
            }
        }
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
    }

    private void Move()
    {
        Vector3 currentPos = transform.position;
        currentPos.x -= MaxSideSpeed * Time.fixedDeltaTime * currentLeanPer;
        currentPos.z -= FlySpeed * Time.fixedDeltaTime;
        rb.MovePosition(currentPos);
        //rb.MovePosition(new Vector3(0f,0f,0f));
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

    public void BoostSpeed()
    {
        baseFlySpeed += speedIncrease;
        actualFlySpeed += speedIncrease;

        pickupParticles.Play();
        //Handheld.Vibrate();

        if (!isBoosting)
        {
            isBoosting = true;
            boostTimer = 0.0f;
        }
    }
}