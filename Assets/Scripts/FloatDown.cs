using UnityEngine;

public class FloatDown : MonoBehaviour
{
    public AnimationCurve speedCurve; // Define the speed over time
    public float duration = 2f; // Total time the movement takes

    private float elapsedTime = 0f; // Tracks how long the object has been moving

    void Update()
    {
        if (elapsedTime < duration)
        {
            // Get the speed from the curve at the current time
            float speed = speedCurve.Evaluate(elapsedTime / duration);

            // Move down based on the curve's speed
            transform.position += Vector3.down * speed * Time.deltaTime;

            // Increment elapsed time
            elapsedTime += Time.deltaTime;
        }
    }
}
