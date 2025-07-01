using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    public Weapon GetWeapon()
    {
        return weapon;
    }
}
