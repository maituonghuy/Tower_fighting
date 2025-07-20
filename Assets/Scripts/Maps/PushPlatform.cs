using UnityEngine;

public class PushPlatform : MonoBehaviour
{
    public Vector2 pushForce = new Vector2(8f, 0f); // Hướng và lực đẩy
    public ForceMode2D forceMode = ForceMode2D.Force;

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.AddForce(pushForce, forceMode);
        }
    }
}
