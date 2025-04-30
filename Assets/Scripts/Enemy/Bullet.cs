using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Speed of the bullet

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime; // Move up the Y-axis
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject); // Destroy the bullet on impact
        }
    }
}
