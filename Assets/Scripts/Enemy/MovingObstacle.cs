using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float moveSpeed = 1.2f;       
    public float fallSpeed = 1f;       
    public bool moveRight = true;

    private Camera mainCamera;
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        mainCamera = Camera.main;

        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            objectWidth = rend.bounds.size.x;
            objectHeight = rend.bounds.size.y;
        }
    }

    void Update()
    {
        Vector3 direction = moveRight ? Vector3.right : Vector3.left;
        transform.position += direction * moveSpeed * Time.deltaTime;

        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        float screenRight = mainCamera.transform.position.x + mainCamera.orthographicSize * mainCamera.aspect - objectWidth / 2f;
        float screenLeft = mainCamera.transform.position.x - mainCamera.orthographicSize * mainCamera.aspect + objectWidth / 2f;

        if (transform.position.x >= screenRight)
        {
            transform.position = new Vector3(screenRight, transform.position.y, transform.position.z);
            moveRight = false;
        }
        else if (transform.position.x <= screenLeft)
        {
            transform.position = new Vector3(screenLeft, transform.position.y, transform.position.z);
            moveRight = true;
        }

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
