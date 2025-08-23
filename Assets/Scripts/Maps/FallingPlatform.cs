using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 0.5f;
    public float destroyDelay = 2f;
    private Rigidbody2D rb;
    private bool triggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!triggered && other.collider.CompareTag("Player"))
        {
            triggered = true;
            Invoke("DropPlatform", fallDelay);
        }
    }

    void DropPlatform()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);
    }
}
