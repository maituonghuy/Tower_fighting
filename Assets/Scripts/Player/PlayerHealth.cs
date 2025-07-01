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

    //Fire 
    private bool isBurning = false;
    private float burnTickInterval = 0.5f;
    private float burnDamage = 3f;
    private float burnDuration = 3f;
    private float burnTimer = 0f;
    private float burnTickTimer = 0f;

    //Stun
    private bool isStunned = false;
    private float stunTimer = 0f;

    //Slow
    private bool isSlowed = false;
    private float slowMultiplier = 1f;
    private float slowTimer = 0f;


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
        //Stun logic
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                Debug.Log("End stun");
            }
        }

        //Slow logic
        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0f)
            {
                isSlowed = false;
                slowMultiplier = 1f;
                Debug.Log("End Slow");
            }
        }
    }

    //Slow
    public void ApplySlow(float multiplier, float duration)
    {
        isSlowed = true;
        slowMultiplier = multiplier;
        slowTimer = duration;
        Debug.Log("Player got slow: x" + multiplier);
    }

    public float GetSpeedMultiplier()
    {
        return slowMultiplier; //
    }

    //Stun
    public void ApplyStun(float duration)
    {
        if (isStunned) return; // 0 cong don

        isStunned = true;
        stunTimer = duration;

        Debug.Log("Player got stun!");
    }

    public bool IsStunned()
    {
        return isStunned;
    }

    //Fire
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


    //--------------------------------------------------------------------------
    //Buff

    //Buff Heal
    public void ApplyHeal(float amount)
    {
        float healedAmount = Mathf.Min(amount, maxHealth - currentHealth);
        currentHealth += healedAmount;

        Debug.Log("Heal got: " + healedAmount + " to Current Heal: " + currentHealth);
    }

}
