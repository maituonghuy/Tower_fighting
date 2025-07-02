using UnityEngine;

public enum BuffType { Active, Passive }

public enum BuffEffectType
{
    IncreaseMaxHealth,
    IncreaseDamage,
    IncreaseMoveSpeed,
    HealthRegen
    // Bạn có thể thêm: Giảm sát thương, Tăng hồi máu, v.v.
}

[CreateAssetMenu(menuName = "TowerGame/Buff")]
public class Buff : Item
{
    public BuffType Type;
    public float duration = 5f;
    public float effectValue = 1.0f;

    public BuffEffectType effectType;

    public override void Activate(PlayerController player)
    {
        if (Type == BuffType.Active)
        {
            Debug.Log($"{player.GetPlayerType()} dùng buff active: {itemName} trong {duration} giây!");
            // Dùng cho buff có thời gian hiệu lực
        }
        else
        {
            Debug.LogWarning("Buff passive không nên gọi Activate() trực tiếp!");
        }
    }
}
