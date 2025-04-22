using UnityEngine;

public class EmptyCheck : MonoBehaviour
{
    void Update()
    {
        // Check if the object has no children
        if (transform.childCount == 0)
        {
            // Destroy this game object
            Destroy(gameObject);
        }
    }
}
