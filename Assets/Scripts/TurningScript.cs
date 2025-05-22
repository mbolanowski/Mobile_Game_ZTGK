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

    public Transform birdRotationTransform;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Rectangle = new Rectangle(XMargin, YMargin, Screen.width - 2 * XMargin, BoxHeight);
        halfXpos = (float)Rectangle.Width * 0.5f;
        xMult = 2.0f / (float)Rectangle.Width;
        baseRotation = birdRotationTransform.localEulerAngles;
        defualtPos = new Vector2(halfXpos, 0);
    }

    // Update is called once per frame
    void Update()
    {
        HandleTouchInput();
    }

    private void FixedUpdate()
    {
        if(leanChanged)UpdateLean();
        Move();
    }

    private void Move()
    {
        Vector3 currentPos = transform.position;
        currentPos.x -= MaxSideSpeed * Time.fixedDeltaTime * currentLeanPer;
        currentPos.z -= FlySpeed * Time.fixedDeltaTime;
        rb.MovePosition(currentPos);
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
        if(targetLeanPer - currentLeanPer < 0)
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
        if(Mathf.Abs(currentLeanPer - targetLeanPer) <= minError)
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
            else if(touch.phase == TouchPhase.Ended)
            {
                targetPos = defualtPos;
                leanChanged = true;
            }
        }
    }
}
