using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public float damage = 10f;
    private PlayerController owner;

    public void SetOwner(PlayerController player, float weaponDamage)
    {
        owner = player;
        damage = weaponDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<PlayerController>();
        if (target != null && target != owner)
        {
            Debug.Log($"Đánh trúng {target.GetPlayerType()} với {damage} sát thương.");
            target.ApplyDamage(damage);
        }
    }
}
