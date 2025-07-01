using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;
    [SerializeField]
    public float jumpForce = 5f;

    private Rigidbody2D rb;

    private PlayerHealth playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.IsStunned())
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); 
            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        float speedMultiplier = 1f;
        PlayerHealth ph = GetComponent<PlayerHealth>();
        if (ph != null) speedMultiplier = ph.GetSpeedMultiplier();

        rb.linearVelocity = new Vector2(moveInput * moveSpeed * speedMultiplier, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

}
