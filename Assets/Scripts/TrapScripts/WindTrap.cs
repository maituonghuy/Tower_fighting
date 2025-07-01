using UnityEngine;

public class WindTrap : MonoBehaviour
{
    public Vector2 pushDirection = new Vector2(1, 1); 
    public float pushForce = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 finalForce = pushDirection.normalized * pushForce;
                rb.linearVelocity = finalForce; 

                Debug.Log("Player got blow!");
            }
        }
    }
}
