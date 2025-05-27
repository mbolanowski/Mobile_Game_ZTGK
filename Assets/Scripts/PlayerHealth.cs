using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHP;
    public float invTimeAfterHit;
    public HealthbarScript healthbarScript;

    private float currentHP;
    private float afterHitTime = 0.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
    }

    private void FixedUpdate()
    {
        afterHitTime += Time.fixedDeltaTime;
    }

    public void TakeDamage(float damage)
    {
        if (afterHitTime < invTimeAfterHit) return;
        afterHitTime = 0.0f;
        currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);
        healthbarScript.ChangeHealthBarState(currentHP/maxHP);
        if (currentHP <= 0)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageSource damageSource = other.gameObject.GetComponent<DamageSource>();
        if(damageSource != null)
        {
            TakeDamage(damageSource.DamageValue);
        }
    }

}
