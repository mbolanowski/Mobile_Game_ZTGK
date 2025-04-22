using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP = 60; // Maximum HP
    private int currentHP;

    public Transform hpBar; // Assign this in the Inspector
    public Renderer hpBarRenderer; // Assign the Renderer of the HP bar (MeshRenderer)

    private Color fullHealthColor = Color.green;  // HP at 100%
    private Color midHealthColor = Color.yellow;  // HP around 50%
    private Color lowHealthColor = Color.red;     // HP at 0%

    public GameObject player;
    public Transform playerTransform; // Reference to the player
    public GameObject bulletPrefab; // Bullet prefab
    public Transform firePoint; // Bullet spawn point
    public float fireRate = 1f; // Bullet fire rate
    private float lastFireTime;

    void Start()
    {
        player = GameObject.Find("Player");
        currentHP = maxHP;
        UpdateHPBar();
    }

    void Update()
    {
        playerTransform = player.transform;
        Debug.Log(playerTransform.position);
        RotateTowardsPlayer();
        ShootAtPlayer();
    }

    public void TakeDamage(int damage)
    {
        if (!hpBar.gameObject.active)
        {
            hpBar.gameObject.SetActive(true);
        }

        currentHP -= damage;
        UpdateHPBar();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void UpdateHPBar()
    {
        if (hpBar != null)
        {
            float hpPercent = (float)currentHP / maxHP;

            // Scale HP bar along X-axis
            hpBar.localScale = new Vector3(hpBar.localScale.x, hpBar.localScale.y, hpPercent / 2f);

            // Change material color
            if (hpBarRenderer != null && hpBarRenderer.material != null)
            {
                if (hpPercent > 0.5f)
                {
                    // Lerp between Green and Yellow
                    hpBarRenderer.material.color = Color.Lerp(midHealthColor, fullHealthColor, (hpPercent - 0.5f) * 2);
                }
                else
                {
                    // Lerp between Yellow and Red
                    hpBarRenderer.material.color = Color.Lerp(lowHealthColor, midHealthColor, hpPercent * 2);
                }
            }
        }
    }

    void Die()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject); // Destroy the parent and all its children
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            TakeDamage(10); // Apply damage
            Destroy(other.gameObject); // Destroy the bullet
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyDeath"))
        {
            Die();
        }
    }

    void RotateTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            // Keep only the X and Y components, ignore Z
            direction.z = 0;

            // Create a rotation where the forward vector points at the player, only rotating around X
            Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, direction);

            // Apply rotation smoothly
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }


    void ShootAtPlayer()
    {
        if (player != null && Time.time - lastFireTime >= fireRate)
        {
            lastFireTime = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Ensure bullet faces the player
            Vector3 direction = (playerTransform.position - firePoint.position).normalized;
            bullet.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
