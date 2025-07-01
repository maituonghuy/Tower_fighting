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

    [Header("State")]
    [SerializeField] private bool isPvP = false;
    private bool isInvincible = false;

    private PlayerAnimationController animationController;

    private GameObject nearbyItem = null;

    [SerializeField] private float jumpForce = 300f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        animationController = GetComponent<PlayerAnimationController>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        HandleMovement();
        HandleSkillInput();
    }

    private void HandleMovement()
    {
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
            if (Input.GetKeyDown(KeyCode.Alpha5)) UseDashSkill();      // Dash: 5
            if (Input.GetKeyDown(KeyCode.Alpha6)) UseUniqueSkill();    // Unique: 6
        }
        else if (playerType == PlayerType.Player2)
        {
            if (Input.GetKeyDown(KeyCode.B)) Attack();
            if (Input.GetKeyDown(KeyCode.N)) UseItem(0);
            if (Input.GetKeyDown(KeyCode.M)) UseItem(1);
            if (Input.GetKeyDown(KeyCode.Comma)) UseItem(2);
            if (Input.GetKeyDown(KeyCode.DownArrow)) TryPickUp();
            if (Input.GetKeyDown(KeyCode.Period)) UseDashSkill();      // Dash: .
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
        // Gọi dash hoặc unique skill tùy theo input
    }

    public void UseItem(int index)
    {
        // Kích hoạt item tại ô slot index (buff active hoặc weapon attack)
    }

    public void ApplyDamage(float damage)
    {
        if (isInvincible) return;
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //GameManager.Instance.OnPlayerDead(playerType);
    }

    public void AddItem(Item newItem)
    {
        if (itemSlots.Count >= 3) return;

        itemSlots.Add(newItem);
        // Cập nhật UI nếu có
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
        // Nếu có vũ khí đang chọn → gọi Activate()
        // Nếu không → chơi animation đánh tay không
        animationController.PlayAttackAnimation();
        Debug.Log($"{playerType} attacked!");
    }

    private void TryPickUp()
    {
        if (nearbyItem == null) return;

        if (nearbyItem.CompareTag("weapon"))
        {
            Weapon weapon = nearbyItem.GetComponent<WeaponPickup>().GetWeapon();
            AddItem(weapon);
            Destroy(nearbyItem);
            nearbyItem = null;
        }
        else if (nearbyItem.CompareTag("buff"))
        {
            Buff buff = nearbyItem.GetComponent<BuffPickup>().GetBuff();

            if (buff.Type == BuffType.Passive)
            {
                AddPassiveBuff(buff); // nếu có hệ thống passive buff

            }
            else
            {
                AddItem(buff); // nếu là buff active
            }

            Destroy(nearbyItem);
            nearbyItem = null;
        }
    }

   private void AddPassiveBuff(Buff buff)
{
    passiveBuffs.Add(buff);

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
    }

    Debug.Log($"{playerType} nhận buff {buff.name}: {buff.effectType} +{buff.effectValue}");
}


    private void UseDashSkill()
    {
        // Gọi dash skill (sau này có thể set cooldown, distance...)
        Debug.Log($"{playerType} used Dash Skill!");
    }

    private void UseUniqueSkill()
    {
        // Gọi skill riêng (tùy theo prefab hoặc skin)
        Debug.Log($"{playerType} used Unique Skill!");
    }

    public PlayerType GetPlayerType() => playerType;
}
