using UnityEngine;

public class DragonController : PlayerController
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireballDelay = 0.25f;

    private float lastFireTime = -Mathf.Infinity;

    protected override void Attack()
    {
        base.Attack();

        if (Time.time >= lastFireTime + fireballDelay)
        {
            ShootFireball();
            lastFireTime = Time.time;
        }
    }

    private void ShootFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        fireball.GetComponent<Fireball>().Launch(direction);

        Vector3 fireScale = fireball.transform.localScale;
        fireScale.x = Mathf.Abs(fireScale.x) * (direction.x > 0 ? 1 : -1);
        fireball.transform.localScale = fireScale;

        animationController.PlayAttackAnimation();
    }
}
