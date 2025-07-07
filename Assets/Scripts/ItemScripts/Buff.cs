using System.Collections;
using UnityEngine;

public enum BuffType { Active, Passive }

public enum BuffEffectType
{
    IncreaseMaxHealth,
    IncreaseDamage,
    IncreaseMoveSpeed,
    HealthRegen,
    Invincible,
    LifeSteal,
    Shield,


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
            switch (effectType)
            {
                case BuffEffectType.Invincible:
                    player.StartCoroutine(InvincibilityCoroutine(player));
                    break;

                case BuffEffectType.LifeSteal:  
                    player.ActivateLifeSteal(effectValue, duration);
                    break;
                case BuffEffectType.Shield:
                    player.ActivateShield(effectValue, duration);
                    break;
            }
            Debug.Log($"{player.GetPlayerType()} dùng buff active: {itemName} trong {duration} giây!");
            // Dùng cho buff có thời gian hiệu lực
        }
        else
        {
            Debug.LogWarning("Buff passive không nên gọi Activate() trực tiếp!");
        }
    }
    private IEnumerator InvincibilityCoroutine(PlayerController player)
    {
        player.SetInvincible(true);
        yield return new WaitForSeconds(duration);
        player.SetInvincible(false);
    }


}
