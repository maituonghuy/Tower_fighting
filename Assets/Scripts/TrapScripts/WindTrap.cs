using UnityEngine;

public class WindTrap : MonoBehaviour
{
    public TrapData trapData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && trapData.effectType == TrapEffectType.Push)
        {
            Vector2 force = Vector2.right * trapData.value; // hoặc chỉnh lại Vector2.up, Vector2.left tùy hướng trap
            rb.linearVelocity = force;
            Debug.Log("Trap triggered: " + trapData.trapName);
        }
    }
}
