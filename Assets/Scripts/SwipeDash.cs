using UnityEngine;

public class SwipeDash : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float fireRate = 0.2f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public LayerMask bounceLayers;

    public float minSwipe = 200f;
    public float maxSwipe = 1000f;

    public AnimationCurve dashSpeedCurve;
    public LineRenderer dashPreview; // Line Renderer for preview

    private Rigidbody rb;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isDashing = false;
    private bool isDragging = false; // Flag to track actual dragging
    private Vector3 dashDirection;
    private float dashTime;
    private float lastFireTime;
    private Vector3 dashStartPosition;
    private Vector3 dashEndPosition;
    private float dashDuration;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        dashPreview.positionCount = 2;
        dashPreview.enabled = false;
    }

    void Update()
    {
        HandleTouchInput();
        //Shoot();
    }

    void FixedUpdate()
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
                dashPreview.enabled = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                endTouchPosition = touch.position;
                if (Vector2.Distance(startTouchPosition, endTouchPosition) >= minSwipe)
                {
                    isDragging = true;
                    UpdateDashPreview();
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                dashPreview.enabled = false;
                if (isDragging)
                {
                    DashInSwipeDirection();
                }
            }
        }
    }

    void UpdateDashPreview()
    {
        Vector2 swipeVector = endTouchPosition - startTouchPosition;
        if (swipeVector.magnitude < minSwipe) return; // Only draw if swipe is large enough

        swipeVector.Normalize();
        dashDirection = new Vector3(-swipeVector.x, swipeVector.y, 0);

        float swipeLength = (endTouchPosition - startTouchPosition).magnitude;
        float minDuration = 0.01f;
        float maxDuration = 0.2f;

        float t = Mathf.InverseLerp(minSwipe, maxSwipe, swipeLength);
        dashDuration = Mathf.Lerp(minDuration, maxDuration, t);

        dashStartPosition = rb.position;
        dashEndPosition = dashStartPosition + dashDirection * dashSpeed * dashDuration;

        RaycastHit hit;
        if (Physics.Raycast(dashStartPosition, dashDirection, out hit, (dashEndPosition - dashStartPosition).magnitude, bounceLayers))
        {
            dashEndPosition = hit.point;
        }

        dashPreview.enabled = true;
        dashPreview.SetPosition(0, dashStartPosition);
        dashPreview.SetPosition(1, dashEndPosition);
    }

    void DashInSwipeDirection()
    {
        isDashing = true;
        dashTime = 0;
    }

    void Dash()
    {
        dashTime += Time.fixedDeltaTime;
        float t = dashTime / dashDuration;
        float curveValue = dashSpeedCurve.Evaluate(t);
        Vector3 nextPosition = Vector3.Lerp(dashStartPosition, dashEndPosition, curveValue);

        rb.MovePosition(nextPosition);

        if (dashTime >= dashDuration)
        {
            isDashing = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing && ((1 << collision.gameObject.layer) & bounceLayers) != 0)
        {
            dashDirection = Vector3.Reflect(dashDirection, collision.contacts[0].normal);
        }
    }

    void Shoot()
    {
        if (Time.time - lastFireTime >= fireRate)
        {
            lastFireTime = Time.time;
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        }
    }
}
