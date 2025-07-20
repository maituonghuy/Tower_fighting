using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 20f;
    public float lifetime = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ApplyDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
