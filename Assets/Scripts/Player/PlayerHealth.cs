using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    public float maxHealth = 100f;
    private float currentHealth;
    [SerializeField]
    public float invincibleDuration = 1.5f;
    private bool isInvincible = false;
    private float invincibleTimer = 0f;

    private bool isBurning = false;
    private float burnTickInterval = 0.5f;
    private float burnDamage = 3f;
    private float burnDuration = 3f;
    private float burnTimer = 0f;
    private float burnTickTimer = 0f;
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
            }
        }

        //Burn logic
        if (isBurning) 
        { 
            burnTimer -= Time.deltaTime;
            burnTickTimer -= Time.deltaTime;

            if(burnTickTimer <= 0f)
            {
                TakeDamage(burnDamage);
                burnTickTimer = burnTickInterval;
            }

            if(burnTimer <= 0f)
            {
                isBurning = false;
                Debug.Log("Burn Done");
            }
        }
    }

    public void ApplyBurn(float damage, float duration)
    {
        if (isBurning) return; //0 cong don

        isBurning = true;
        burnDamage = damage;
        burnDuration = duration;
        burnTimer = duration;
        burnTickTimer = 0f;

        Debug.Log("Player got Burn");
    }
    public void TakeDamage(float amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        Debug.Log("Player bi thuong:" + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        isInvincible = true;
        invincibleTimer = invincibleDuration;
    }
    public void Die()
    {
        Debug.Log("Die");
        Destroy(gameObject);
    }
}
