using UnityEngine;

public enum TrapEffectType
{
    Stun,        // Làm choáng
    Burn,        // Thiêu đốt theo thời gian
    Slow,        // Giảm tốc độ di chuyển
    Push,        // Đẩy người chơi đi
    Damage       // Gây sát thương trực tiếp
}

[CreateAssetMenu(menuName = "TowerGame/Trap")]
public class TrapData : ScriptableObject
{
    [Header("Thông tin cơ bản")]
    public string trapName;
    public Sprite icon;

    [Header("Hiệu ứng Trap")]
    public TrapEffectType effectType;

    [Tooltip("Giá trị chính của trap:\n- Damage = sát thương\n- Slow = phần trăm giảm tốc (0.5 = 50%)\n- Push = lực đẩy\n- Burn = sát thương mỗi tick")]
    public float value = 0f;

    [Tooltip("Thời gian hiệu lực:\n- Stun = thời gian choáng\n- Burn = tổng thời gian thiêu đốt\n- Slow = thời gian giảm tốc")]
    public float duration = 0f;

    [Tooltip("Thời gian delay giữa các lần gây hiệu ứng (chỉ dùng cho Burn hoặc Slow)")]
    public float tickInterval = 1f; // chỉ dùng nếu bạn xử lý trap burn kiểu lặp

}
