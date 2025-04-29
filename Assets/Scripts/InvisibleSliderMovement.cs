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
        Vector3 newPos = CanUseWholeScreen ? currentPos + offset  : centerPos + offset;
        MoveTime += Time.fixedDeltaTime;
        float t = MoveTime / MoveDelay;
        float curveValue = moveSpeedCurve.Evaluate(t);
        Vector3 nextPosition = Vector3.Lerp(currentPos, newPos, curveValue);

        rb.MovePosition(nextPosition);
        if (MoveTime >= MoveDelay)
        {
            moving = false;
        }
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
    }
}
