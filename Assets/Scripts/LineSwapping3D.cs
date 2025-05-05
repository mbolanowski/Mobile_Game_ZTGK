using System;
using UnityEngine;

public class LineSwapping3D : MonoBehaviour
{
    [Header("Swipping parameters")]
    public float minSwipeDistance = 100.0f;
    public float horizontalBias = 1.0f;
    public float LineSwapTime;
    public AnimationCurve LineSwapMovementCurve;

    [Header("Horizontal parameters")]
    public float horizontalSpan;
    public int horizontalLanesFromCenter;

    [Header("Vertical parameters")]
    public float verticalSpan;
    public int verticalLanes;
    public bool addLanesBellow = true;

    private Rigidbody rb;

    private Vector3 centralPos;
    private Vector3 horizontalUnitOffset;
    private Vector3 verticalUnitOffset;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool isDragging = false;
    private bool isDashing = false;

    private float currentLineSwapTime;

    private int minVert;
    private int maxVert;

    private int currentHorizontalLane = 0;
    private int currentVerticalLane = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        centralPos = transform.position;
        horizontalUnitOffset.x = -horizontalSpan / (horizontalLanesFromCenter * 2);
        verticalUnitOffset.z = (addLanesBellow?-1.0f: 1.0f) * (verticalSpan / (verticalLanes - 1));
        minVert = addLanesBellow ? -verticalLanes + 1 : 0;
        maxVert = addLanesBellow ? 0 : verticalLanes - 1;
    }

    // Update is called once per frame
    void Update()
    {
        HandleTouchInput();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            ExecuteMovement();
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = touch.position;
                endTouchPos = startTouchPos; // Prevent old values
                isDragging = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                endTouchPos = touch.position;
                if (Vector2.Distance(startTouchPos, endTouchPos) >= minSwipeDistance)
                {
                    isDragging = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (isDragging)
                {
                    DecideDirection();
                }
            }
        }
    }

    void DecideDirection()
    {
        startPosition = rb.position;
        Vector2 swipeDir = endTouchPos - startTouchPos;
        swipeDir.Normalize();
        if(Mathf.Abs(swipeDir.x) * horizontalBias > Mathf.Abs(swipeDir.y))
        {
            int newLevel = currentHorizontalLane + (swipeDir.x > 0 ? 1 : -1);
            if (Mathf.Abs(newLevel) <= horizontalLanesFromCenter)
            {
                currentHorizontalLane = newLevel;
                CalculateNewtargetPos();
                currentLineSwapTime = 0;
                isDashing = true;
            }
        } 
        else
        {
            int newLevel = currentVerticalLane + (swipeDir.y > 0 ? 1 : -1);
            if(newLevel >= minVert && newLevel <= maxVert)
            {
                currentVerticalLane = newLevel;
                CalculateNewtargetPos();
                currentLineSwapTime = 0;
                isDashing = true;
            }
        }
    }

    void CalculateNewtargetPos()
    {
        targetPosition = centralPos + (horizontalUnitOffset * currentHorizontalLane) + (verticalUnitOffset * currentVerticalLane);
    }

    void ExecuteMovement()
    {
        currentLineSwapTime += Time.fixedDeltaTime;
        float t = currentLineSwapTime / LineSwapTime;
        float curveValue = LineSwapMovementCurve.Evaluate(t);
        Vector3 nextPosition = Vector3.Lerp(startPosition, targetPosition, curveValue);

        rb.MovePosition(nextPosition);

        if (currentLineSwapTime >= LineSwapTime)
        {
            isDashing = false;
        }
    }

}
