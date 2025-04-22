using UnityEngine;

public class EndlessScroll : MonoBehaviour
{
    public float speed = 5f; // Speed of movement
    private Camera mainCamera;
    private float objectHeight; // Height of the object

    void Start()
    {
        mainCamera = Camera.main;
        objectHeight = GetComponent<Renderer>().bounds.size.y; // Get object height
    }

    void Update()
    {
        // Move the object down
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Calculate the bottom boundary of the camera
        float screenBottom = mainCamera.transform.position.y - mainCamera.orthographicSize - objectHeight / 2;

        // If the object is fully out of view at the bottom, move it to the top
        if (transform.position.y < screenBottom)
        {
            float screenTop = mainCamera.transform.position.y + mainCamera.orthographicSize + objectHeight / 2;
            transform.position = new Vector3(transform.position.x, screenTop, transform.position.z);
        }
    }
}
