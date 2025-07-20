using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using static UnityEditor.Progress;

public enum PlayerType { Player1, Player2 }

public class PlayerController : MonoBehaviour
{
    [Header("Player Identity")]
    [SerializeField] private PlayerType playerType;

    [Header("Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float moveSpeed;

    [Header("Combat")]
    [SerializeField] private float baseDamage = 10f;  // damage gốc
    private float currentDamage;


    private Rigidbody2D rb;
    private BoxCollider2D col;

    [Header("Skills")]
    [SerializeField] private Skill dashSkill; // Dash dùng chung cho mọi player
    [SerializeField] private Skill uniqueSkill; // Skill riêng theo sprite

    [Header("Items")]
    [SerializeField] private List<Item> itemSlots = new List<Item>(3);
    // Gồm weapon hoặc buff active

    [Header("Buffs")]
    [SerializeField] private List<Buff> passiveBuffs; // Chỉ chứa buff passive

    //Trap
    [Header("Traps")]
    [SerializeField] private List<TrapData> activeTrapEffects = new List<TrapData>();

    private bool isStunned = false;
    private bool isBurning = false;
    private bool isSlowed = false;

    [Header("State")]
    [SerializeField] private bool isPvP = false;
    private bool isInvincible = false;

    [SerializeField] private Transform weaponHolder;
    private GameObject currentWeapon;
    private int equippedItemIndex = -1; // -1 nghĩa là không cầm gì

    [SerializeField] private GameObject bulletPrefab;



    private PlayerAnimationController animationController;

    private GameObject nearbyItem = null;

    [SerializeField] private float jumpForce = 300f;

    //Buff Active hút máu
    private bool isLifeStealing = false;
    private float lifeStealPercent = 0f;

    //shield Active
    private float damageReductionPercent = 0f;

    //UI
    public UIController itemUI;
    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 50f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing = false;
    private float lastDashTime = -10f; // Initialize to allow first dash immediately

    // Speed modifier system
    private Dictionary<string, float> speedModifiers = new Dictionary<string, float>();
    private float baseSpeed;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        animationController = GetComponent<PlayerAnimationController>();
        currentHealth = maxHealth;

        while (itemSlots.Count < 3)
        {
            itemSlots.Add(null);
        }

        InitializeSpeedManager();
    }

    void Update()
    {
        if (!isStunned)
            HandleMovement();
        HandleSkillInput();
    }

    private void InitializeSpeedManager()
    {
        baseSpeed = moveSpeed;
        speedModifiers.Clear();
    }

    private void HandleMovement()
    {
        if (isStunned) return;

        float moveX = 0f;

        if (playerType == PlayerType.Player1)
        {
            moveX = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;

            if (Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }
        }
        else if (playerType == PlayerType.Player2)
        {
            moveX = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
        }

        Vector2 move = new Vector2(moveX, rb.linearVelocity.y); // giữ nguyên trục Y
        rb.linearVelocity = new Vector2(move.x * moveSpeed, rb.linearVelocity.y);

        // Animation
        if (moveX != 0)
        {
            animationController.PlayRunAnimation();

            // Quay đầu nhân vật dựa trên hướng di chuyển
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (moveX > 0 ? 1 : -1);
            transform.localScale = scale;
        }
        else
        {
            animationController.StopRunAnimation();
        }

        HandleInput(); // xử lý tấn công, skill, item
    }

    private void HandleInput()
    {
        if (playerType == PlayerType.Player1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) Attack();
            if (Input.GetKeyDown(KeyCode.Alpha2)) UseItem(0);
            if (Input.GetKeyDown(KeyCode.Alpha3)) UseItem(1);
            if (Input.GetKeyDown(KeyCode.Alpha4)) UseItem(2);
            if (Input.GetKeyDown(KeyCode.S)) TryPickUp();
            if (UnityEngine.InputSystem.Keyboard.current.zKey.wasReleasedThisFrame) UseDashSkill();      // Dash: 5
            if (Input.GetKeyDown(KeyCode.Alpha6)) UseUniqueSkill();    // Unique: 6
        }
        else if (playerType == PlayerType.Player2)
        {
            if (Input.GetKeyDown(KeyCode.B)) Attack();
            if (Input.GetKeyDown(KeyCode.N)) UseItem(0);
            if (Input.GetKeyDown(KeyCode.M)) UseItem(1);
            if (Input.GetKeyDown(KeyCode.Comma)) UseItem(2);
            if (Input.GetKeyDown(KeyCode.DownArrow)) TryPickUp();
            if (UnityEngine.InputSystem.Keyboard.current.mKey.wasReleasedThisFrame) UseDashSkill();      // Dash: .
            if (Input.GetKeyDown(KeyCode.Slash)) UseUniqueSkill();     // Unique: /
        }
    }

    private void Jump()
    {
        // Chỉ nhảy nếu đang chạm đất (tuỳ bạn muốn kiểm tra bằng Raycast hay Trigger)
        animationController.PlayJumpAnimation();
        rb.AddForce(Vector2.up * jumpForce);
        Debug.Log($"{playerType} jumped!");
    }


    private void HandleSkillInput()
    {
        //Trigger SkillExecutor
        // Gọi dash hoặc unique skill tùy theo input
    }

    public void UseItem(int index)
    {
        if (index < 0 || index >= itemSlots.Count) return;

        Item item = itemSlots[index];
        if (item == null) return;

        // Nếu đang cầm chính item này → bỏ xuống
        if (equippedItemIndex == index)
        {
            UnequipWeapon(); // Xóa vũ khí khỏi tay
            equippedItemIndex = -1;
            return;
        }

        // Nếu là Weapon → trang bị
        if (item is Weapon weapon)
        {
            EquipWeapon(weapon);
            equippedItemIndex = index;
        }
        else if (item is Buff buff && buff.Type == BuffType.Active)
        {
            item.Activate(this); // Dùng buff

            itemSlots[index] = null; // Dùng xong thì mất
            if (itemUI != null)
                itemUI.ClearItemSlot(index);

            equippedItemIndex = -1;
        }
    }

    private void UnequipWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }
    }


    public void ApplyDamage(float damage)
    {
        if (isInvincible) return;

        float actualDamage = damage * (1f - damageReductionPercent);
        currentHealth -= actualDamage;

        if (currentHealth <= 0)
        {
            Die();
        }

        isInvincible = true;
        Invoke(nameof(ResetInvincibility), 1.5f);
    }

    private void ResetInvincibility()
    {
        isInvincible = false;
    }

    private void Die()
    {
        //GameManager.Instance.OnPlayerDead(playerType);
    }

    public bool AddItem(Item newItem)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i] == null)
            {
                itemSlots[i] = newItem;
                if (itemUI != null)
                    itemUI.SetItemIcon(i, newItem.icon);
                return true;
            }
        }

        // Nếu tất cả slot đều đang full → không thêm được
        Debug.Log($"{playerType}: Không còn slot trống để thêm item.");
        return false;
    }

    public void SetPvPMode(bool value)
    {
        isPvP = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("weapon") || other.CompareTag("buff"))
        {
            nearbyItem = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == nearbyItem)
        {
            nearbyItem = null;
        }
    }

    private void Attack()
    {
        if (currentWeapon != null)
        {
            Weapon weapon = itemSlots[equippedItemIndex] as Weapon;

            Animator weaponAnimator = currentWeapon.GetComponent<Animator>();
            if (weaponAnimator != null && weapon != null && !string.IsNullOrEmpty(weapon.weaponAnimatorTrigger))
            {
                weaponAnimator.SetTrigger(weapon.weaponAnimatorTrigger);
            }

            // Nếu là súng thì bắn
            if (weapon.name.Contains("Gun"))
            {
                FireBullet(weapon);
            }
            else
            {
                weapon.Activate(this);
            }
        }
        else
        {
            animationController.PlayAttackAnimation();
        }

        Debug.Log($"{playerType} attacked!");
    }

    private void FireBullet(Weapon weapon)
    {
        if (bulletPrefab == null) return;

        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Tìm điểm bắn từ trong vũ khí đang cầm
        Transform shootPoint = currentWeapon.transform.Find("ShootPoint");

        Vector3 spawnPos = shootPoint != null ? shootPoint.position : weaponHolder.position;

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(direction);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
            bulletScript.SetOwner(this, weapon.damage);
        }

        }

    private void TryPickUp()
    {
        if (nearbyItem == null) return;

        if (nearbyItem.CompareTag("weapon"))
        {
            Weapon weapon = nearbyItem.GetComponent<WeaponPickup>().GetWeapon();
            if (AddItem(weapon)) // chỉ xóa nếu nhặt thành công
            {
                Destroy(nearbyItem);
                nearbyItem = null;
            }
        }
        else if (nearbyItem.CompareTag("buff"))
        {
            Buff buff = nearbyItem.GetComponent<BuffPickup>().GetBuff();

            if (buff.Type == BuffType.Passive)
            {
                AddPassiveBuff(buff);
                Destroy(nearbyItem);
                nearbyItem = null;
            }
            else
            {
                if (AddItem(buff)) //chỉ xóa nếu nhặt thành công
                {
                    Destroy(nearbyItem);
                    nearbyItem = null;
                }
            }
        }
    }


    private void AddPassiveBuff(Buff buff)
    {
        switch (buff.effectType)
        {
            case BuffEffectType.IncreaseMaxHealth:
                maxHealth += buff.effectValue;
                currentHealth += buff.effectValue; // có thể cộng ngay HP nếu muốn
                break;

            case BuffEffectType.IncreaseDamage:
                baseDamage += buff.effectValue;
                currentDamage = baseDamage;
                break;

            case BuffEffectType.IncreaseMoveSpeed:
                moveSpeed += buff.effectValue;
                break;

            case BuffEffectType.HealthRegen:
                StartCoroutine(HealthRegenCoroutine(buff.effectValue, buff.duration));
                break;
        }

        Debug.Log($"{playerType} nhận buff {buff.name}: {buff.effectType} +{buff.effectValue}");
    }

    private void UseDashSkill()
    {
        Debug.Log($"{playerType} đang sử dụng Dash Skill...");
        // Gọi dash skill (sau này có thể set cooldown, distance...)
        if (isDashing || Time.time < lastDashTime + dashCooldown) return;

        isDashing = true;
        lastDashTime = Time.time;


        Vector2 dashDirection = (playerType == PlayerType.Player1) ?
            new Vector2(Input.GetAxis("Horizontal"), 0).normalized :
            new Vector2(Input.GetAxis("Horizontal_P2"), 0).normalized;

        rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);

        Invoke(nameof(ResetDash), dashDuration);
        Debug.Log($"{playerType} used Dash Skill!");
    }

    private void ResetDash()
    {
        isDashing = false;
    }

    private void UseUniqueSkill()
    {
        // Gọi skill riêng (tùy theo prefab hoặc skin)
        Debug.Log($"{playerType} used Unique Skill!");
    }

    public void EquipWeapon(Weapon weapon)
    {

        // Xóa vũ khí cũ nếu có
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        if (weapon != null && weapon.weaponPrefab != null)
        {
            // Tạo vũ khí mới tại vị trí WeaponHolder
            currentWeapon = Instantiate(weapon.weaponPrefab, weaponHolder);
            currentWeapon.transform.localScale = Vector3.one * 5f;
            currentWeapon.transform.localPosition = Vector3.zero;

            WeaponHitbox hitbox = currentWeapon.GetComponentInChildren<WeaponHitbox>();
            if (hitbox != null)
            {
                hitbox.SetOwner(this, weapon.damage);
            }
        }

    }

    public PlayerType GetPlayerType() => playerType;

    

    public void ApplyTrapEffect(TrapData trapData)
    {
        if (!activeTrapEffects.Contains(trapData))
            activeTrapEffects.Add(trapData);

        switch (trapData.effectType)
        {
            case TrapEffectType.Damage:
                ApplyDamage(trapData.value);
                break;
            case TrapEffectType.Burn:
                ApplyBurn(trapData.value, trapData.duration, trapData);
                break;

            case TrapEffectType.Stun:
                ApplyStun(trapData.duration, trapData);
                break;

            case TrapEffectType.Slow:
                ApplySlow(trapData.value, trapData.duration, trapData);
                break;
            case TrapEffectType.Push:
                rb.linearVelocity = Vector2.up * trapData.value;
                break;
        }
    }

    private void ApplySlow(float value, float duration, TrapData trapData)
    {
        if (isSlowed) return;

        StartCoroutine(SlowCoroutine(value, duration, trapData));
    }

    private IEnumerator SlowCoroutine(float value, float duration, TrapData trapData)
    {
        isSlowed = true;
        float originalSpeed = moveSpeed;
        moveSpeed *= value;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed;
        isSlowed = false;
        activeTrapEffects.Remove(trapData);
    }

    private void ApplyBurn(float value, float duration, TrapData trapData)
    {
        if (isBurning) return;

        StartCoroutine(BurnCoroutine(value, duration, trapData));
    }


    private IEnumerator BurnCoroutine(float damagePerTick, float totalDuration, TrapData trapData)
    {
        isBurning = true;
        float tickInterval = 1f;
        float timer = totalDuration;

        while (timer > 0)
        {
            ApplyDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
            timer -= tickInterval;
        }

        isBurning = false;
        activeTrapEffects.Remove(trapData);
    }

    private void ApplyStun(float duration, TrapData trapData)
    {
        if (isStunned) return;

        StartCoroutine(StunCoroutine(duration, trapData));
    }


    private IEnumerator StunCoroutine(float duration, TrapData trapData)
    {
        isStunned = true;
        float originalSpeed = moveSpeed;
        moveSpeed = 0;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed;
        isStunned = false;
        activeTrapEffects.Remove(trapData);
    }

    private IEnumerator HealthRegenCoroutine(float healPerSecond, float duration)
    {
        float timer = 0f;
        float interval = 1f; // hồi máu mỗi giây

        while (timer < duration)
        {
            currentHealth += healPerSecond;
            currentHealth = Mathf.Min(currentHealth, maxHealth); // không vượt quá max

            Debug.Log($"{playerType} hồi {healPerSecond} máu. Máu hiện tại: {currentHealth}");

            yield return new WaitForSeconds(interval);
            timer += interval;
        }
    }
    public void SetPlayerType(PlayerType type)
    {
        this.playerType = type;
    }

    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }


    public void ActivateLifeSteal(float percent, float duration)
    {
        if (isLifeStealing) return;

        StartCoroutine(LifeStealCoroutine(percent, duration));
    }

    private IEnumerator LifeStealCoroutine(float percent, float duration)
    {
        isLifeStealing = true;
        lifeStealPercent = percent;

        yield return new WaitForSeconds(duration);

        isLifeStealing = false;
        lifeStealPercent = 0f;
    }

    public void ActivateShield(float percent, float duration)
    {
        StartCoroutine(ShieldCoroutine(percent, duration));
    }

    private IEnumerator ShieldCoroutine(float percent, float duration)
    {
        damageReductionPercent = percent;

        yield return new WaitForSeconds(duration);

        damageReductionPercent = 0f;
    }

    //public void SetPlayerType(PlayerType type)
    //{
    //    playerType = type;
    //}  
    public void ApplySpeedModifier(string source, float multiplier, float duration = -1f)
    {
        speedModifiers[source] = multiplier;
        RecalculateSpeed();
        Debug.Log($"Applied speed modifier '{source}': {multiplier}x to {playerType}");
    }

    public void RemoveSpeedModifier(string source)
    {
        if (speedModifiers.ContainsKey(source))
        {
            speedModifiers.Remove(source);
            RecalculateSpeed();
            Debug.Log($"Removed speed modifier '{source}' from {playerType}");
        }
    }

    private void RecalculateSpeed()
    {
        float newSpeed = baseSpeed;
        foreach (var modifier in speedModifiers.Values)
        {
            newSpeed *= modifier;
        }
        moveSpeed = newSpeed;
    }

    public void UpdateMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}
