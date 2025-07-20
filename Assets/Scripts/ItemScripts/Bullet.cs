using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    private float damage;
    public float lifetime = 2f;

    private Vector2 direction;
    private PlayerController owner; //  Thêm chủ sở hữu để tránh tự bắn mình

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime); // tự hủy sau thời gian
    }

    public void SetOwner(PlayerController shooter, float dmg)
    {
        owner = shooter;
        damage = dmg;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null && pc != owner) // tránh bắn chính mình
            {
                pc.ApplyDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (!collision.isTrigger)
        {
            Destroy(gameObject); // bắn vào tường cũng tự hủy
        }
    }
}
