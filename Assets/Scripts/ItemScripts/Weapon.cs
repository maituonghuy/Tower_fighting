using UnityEngine;

[CreateAssetMenu(menuName = "TowerGame/Weapon")]
public class Weapon : Item
{
    public float damage = 10f;

    public override void Activate(PlayerController player)
    {
        Debug.Log($"{player.GetPlayerType()} dùng vũ khí gây {damage} damage!");
        // Sau này: play animation, tạo hitbox, v.v.
    }
}
