using UnityEngine;

[CreateAssetMenu(menuName = "TowerGame/Weapon")]
public class Weapon : Item
{
    public float damage = 10f;
    public GameObject weaponPrefab; // này để chứa prefab thanh kiếm
    public string weaponAnimatorTrigger = "Swing";
    public Transform shootPoint;


    public override void Activate(PlayerController player)
    {
        Debug.Log($"{player.GetPlayerType()} dùng vũ khí gây {damage} damage!");
        player.EquipWeapon(this); // Gọi phương thức trang bị vũ khí
    }
}
