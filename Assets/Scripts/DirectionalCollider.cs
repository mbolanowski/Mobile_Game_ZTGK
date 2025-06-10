using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DirectionalCollider : MonoBehaviour
{
    [Tooltip("Minimum angle (in degrees) to consider the trigger entered from the opposite direction.")]
    public float minOppositeAngle = 120f;

    private void OnTriggerEnter(Collider other)
    {
        // Get the direction from this object to the other object
        Vector3 directionToOther = (other.transform.position - transform.position).normalized;

        // Get this object's local right direction (red arrow)
        Vector3 rightDirection = transform.right;

        // Calculate the angle between them
        float angle = Vector3.Angle(rightDirection, directionToOther);

        // Check if the entry is from the opposite direction
        if (angle >= minOppositeAngle)
        {
            Debug.Log("Trigger entered from the opposite direction!");
            // Put your activation logic here
        }
    }
}
