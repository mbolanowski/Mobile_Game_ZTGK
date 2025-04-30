using UnityEngine;

public class StaticObstacle : MonoBehaviour
{
    public float fallSpeed = 2f;

    private Camera mainCamera;
    private float objectHeight;

    void Start()
    {
        mainCamera = Camera.main;
        objectHeight = GetComponent<Renderer>().bounds.size.y;
    }

    void Update()
    {
        // Ruch w dół
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Zniszcz przeszkodę, jeśli wypadnie poza ekran
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
            Debug.Log("Gracz uderzył w statyczną przeszkodę!");
            Destroy(gameObject); 
        }
    }
}