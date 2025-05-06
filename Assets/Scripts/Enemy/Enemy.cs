using System.Drawing;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public int maxHP = 60; 
    private int currentHP;

    public Transform hpBar;
    public Renderer hpBarRenderer;

    private UnityEngine.Color fullHealthColor = UnityEngine.Color.green;
    private UnityEngine.Color midHealthColor = UnityEngine.Color.yellow;
    private UnityEngine.Color lowHealthColor = UnityEngine.Color.red;

    public string alphaPropertyName = "_Alpha";

    public GameObject player;
    public Transform playerTransform;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f; 
    private float lastFireTime;

    public bool shooting = false;
    public bool rotating = false;

    void Start()
    {
        player = GameObject.Find("Player");
        currentHP = maxHP;
        UpdateHPBar();
    }

    void Update()
    {
        playerTransform = player.transform;
        if(rotating) RotateTowardsPlayer();
        if(shooting) ShootAtPlayer();

        if(Mathf.Abs(player.transform.position.z - transform.position.z) > 1f)
        {
            Material material = gameObject.GetComponent<Renderer>().material;
            material.DOFloat(0.2f, alphaPropertyName, 0.5f);
        }
        else
        {
            Material material = gameObject.GetComponent<Renderer>().material;
            material.DOFloat(1.0f, alphaPropertyName, 0.5f);
        }
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
                    hpBarRenderer.material.color = UnityEngine.Color.Lerp(midHealthColor, fullHealthColor, (hpPercent - 0.5f) * 2);
                }
                else
                {
                    // Lerp between Yellow and Red
                    hpBarRenderer.material.color = UnityEngine.Color.Lerp(lowHealthColor, midHealthColor, hpPercent * 2);
                }
            }
        }
    }

    void Die()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
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
