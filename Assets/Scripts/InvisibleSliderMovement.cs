using System;
using System.Drawing;
using TMPro;
using UnityEditor;
using UnityEngine;

public class InvisibleSliderMovement : MonoBehaviour
{
    public bool CanUseWholeScreen = false;

    public GameObject Plane;

    Vector3 targetPos;
    Vector3 centerPos;
    private Rigidbody rb;
    bool moving = false;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    public float minSwipe = 25f;
    public float minimumDIstance = 0.5f;
    public float maxSpeed = 25.0f;

    public float WorldWidth;
    public float WorldHeight;
    public int XMargin;
    public int YMargin;
    public int BoxHeight;
    private float ScreenWorldWidthRatio;
    private float ScreenWorldHeightRatio;
    private Rectangle Rectangle;

    public float MoveDelay;
    private float MoveTime;
    private float ScreenRatio;

    public AnimationCurve moveSpeedCurve;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        centerPos = rb.position;
        ScreenRatio = (float)Screen.width / (float)Screen.height;
        Debug.Log(ScreenRatio);
        WorldHeight = WorldWidth / ScreenRatio;
        ScreenWorldHeightRatio = WorldHeight / Screen.height;
        ScreenWorldWidthRatio = WorldWidth/ Screen.width;
        Rectangle = new Rectangle(XMargin, YMargin, Screen.width - 2 * XMargin, BoxHeight);

        Vector3 planePos = Plane.transform.position;
        planePos.y = -9.0f + ((float)(BoxHeight + YMargin) / (float)Screen.height) * 10.0f;
        Plane.transform.position = planePos;
    }

    // Update is called once per frame
    void Update()
    {
        HandleTouchInput();
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            Move();
        }
    }

    private void Move()
    {
        float xPos;
        float yPos;
        if (CanUseWholeScreen)
        {
            xPos = (endTouchPosition.x * ScreenWorldWidthRatio) - 0.5f * WorldWidth;
            yPos = (endTouchPosition.y * ScreenWorldHeightRatio) - 0.5f * WorldHeight;
        }
        else
        {
            if (!Rectangle.Contains((int)endTouchPosition.x, (int)endTouchPosition.y)) return;
            xPos = (endTouchPosition.x * ScreenWorldWidthRatio) - 0.5f * WorldWidth;
            yPos = 0;

        }
        Vector3 currentPos = rb.position;
        Vector3 offset = new Vector3(-xPos, yPos, 0);
        Vector3 dest = CanUseWholeScreen ? offset + new Vector3(0,0,1.19f)  : centerPos + offset;
        dest.z = 1.19f;
        Vector3 dir = (dest - currentPos);
        dir.Normalize();
        MoveTime += Time.fixedDeltaTime;
        float t = MoveTime / MoveDelay;
        float curveValue = moveSpeedCurve.Evaluate(t);
        dir *= curveValue * maxSpeed * Time.fixedDeltaTime;
        Vector3 finalNewPos;
        if ((dest - currentPos).magnitude <= dir.magnitude || (dest - currentPos).magnitude <= minimumDIstance)
        {
            finalNewPos = dest;
            moving = false;
        }
        else
        {
            finalNewPos = currentPos + dir;
        }
        //Vector3 nextPosition = Vector3.Lerp(currentPos, finalNewPos, curveValue);
        rb.MovePosition(finalNewPos);
    }

    public void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                endTouchPosition = touch.position;
                moving = true;
                MoveTime = 0;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                endTouchPosition = touch.position;
                if (Vector2.Distance(startTouchPosition, endTouchPosition) >= minSwipe)
                {
                    moving = true;
                    MoveTime = 0;
                }
            }
        }
        else
        {
            // Reset to default Z when no touch input
            Vector3 currentPos = rb.position;
            currentPos.z = 3.19f;
            rb.MovePosition(currentPos);
        }
    }
}
