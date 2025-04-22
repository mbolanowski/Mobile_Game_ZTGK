using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f; // Bullet speed
    public float lifetime = 3f; // How long the bullet lasts
    private Transform player;
    private Vector3 direction;

    void Start()
    {
        player = GameObject.Find("Player")?.transform;

        direction = (player.position - transform.position).normalized;
    }

    void Update()
    {
        if (player != null)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            // If the player is missing, just move forward
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject); // Destroy the bullet on impact
        }
    }
}
