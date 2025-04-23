using UnityEditor;
using UnityEngine;

public class LineSwaping : MonoBehaviour
{
    public float dashSpeed = 10f;

    public float MovementDelay;
    public int linesFromMiddle;
    public float totalWidth;

    public float minSwipe = 200f;
    public float maxSwipe = 1000f;

    private int CurrentLine = 0;

    public AnimationCurve dashSpeedCurve;

    private Rigidbody rb;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isDashing = false;
    private bool isDragging = false; // Flag to track actual dragging
    private Vector3 dashDirection;
    private float dashTime;
    private Vector3 dashStartPosition;
    private Vector3 dashEndPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
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
            Dash();
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                endTouchPosition = startTouchPosition; // Prevent old values
                isDragging = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                endTouchPosition = touch.position;
                if (Vector2.Distance(startTouchPosition, endTouchPosition) >= minSwipe)
                {
                    isDragging = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (isDragging)
                {
                    DashInSwipeDirection();
                }
            }
        }
    }
    void DashInSwipeDirection()
    {
        dashStartPosition = rb.position;
        dashEndPosition = rb.position;
        Vector2 change = endTouchPosition - startTouchPosition;
        if(Mathf.Abs(CurrentLine + (change.x > 0 ? 1 : -1)) <= linesFromMiddle)
        {
            CurrentLine += (change.x > 0 ? 1 : -1);
            dashEndPosition.x += (change.x > 0 ? -1 : 1) * (totalWidth / (2 * linesFromMiddle + 1));
            isDashing = true;
            dashTime = 0;
        }
    }

    void Dash()
    {
        dashTime += Time.fixedDeltaTime;
        float t = dashTime / MovementDelay;
        float curveValue = dashSpeedCurve.Evaluate(t);
        Vector3 nextPosition = Vector3.Lerp(dashStartPosition, dashEndPosition, curveValue);

        rb.MovePosition(nextPosition);

        if (dashTime >= MovementDelay)
        {
            isDashing = false;
        }
    }
}
