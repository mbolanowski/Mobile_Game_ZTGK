using UnityEngine;

public class StaticObstacle : MonoBehaviour
{
    public float fallSpeed = 1.0f;

    private Camera mainCamera;
    private float objectHeight;

    void Start()
    {
        mainCamera = Camera.main;
        objectHeight = GetComponent<Renderer>().bounds.size.y;
    }

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        float screenBottom = mainCamera.transform.position.y - mainCamera.orthographicSize - objectHeight / 2f;

        if (transform.position.y < screenBottom)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); 
        }
    }
}