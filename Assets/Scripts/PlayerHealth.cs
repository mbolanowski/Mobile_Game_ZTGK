using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHP;
    public float invTimeAfterHit;
    public HealthbarScript healthbarScript;

    public float healthLossRate;

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
        ModifyHealth(-healthLossRate * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        if (afterHitTime < invTimeAfterHit || damage < 0.0f) return;
        afterHitTime = 0.0f;
        ModifyHealth(-damage);
    }

    private void ModifyHealth(float change)
    {
        currentHP = Mathf.Clamp(currentHP + change,0, maxHP);
        healthbarScript.ChangeHealthBarState(currentHP / maxHP);
        if (currentHP <= 0)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }

    public void Heal(float healValue)
    {
        if(healValue < 0.0f) return;
        ModifyHealth(healValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageSource damageSource = other.gameObject.GetComponent<DamageSource>();
        if(damageSource != null)
        {
            TakeDamage(damageSource.DamageValue);
            Destroy(damageSource.gameObject);
        }
    }

}
