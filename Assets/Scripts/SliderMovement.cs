using System;
using UnityEditor;
using UnityEngine;

public class SliderMovement : MonoBehaviour
{

    public float MaxHorizontalOffset;
    [Min(0.00001f)]
    public float MovementDelay;

    public AnimationCurve movementCurve;

    Vector3 defaultPosition;
    Vector3 targetPosition;
    Vector3 previousTarget;
    private Rigidbody rb;
    private float SliderValue;

    private bool moving = false;
    private float moveTime = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        defaultPosition = transform.position;
        previousTarget = transform.position;
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
        moveTime += Time.fixedDeltaTime;
        float t = moveTime / MovementDelay;
        float curveValue = movementCurve.Evaluate(t);
        Vector3 nextPosition = Vector3.Lerp(previousTarget, targetPosition, curveValue);

        rb.MovePosition(nextPosition);

        if (moveTime >= MovementDelay)
        {
            moving = false;
        }
    }

    public void HandleSliderChange(System.Single value)
    {
        moving = true;
        moveTime = 0;
        previousTarget = transform.position;
        targetPosition = defaultPosition;
        targetPosition.x += value * MaxHorizontalOffset;
    }
}
