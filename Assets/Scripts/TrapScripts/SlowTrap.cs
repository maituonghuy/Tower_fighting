using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    public float slowMultiplier = 0.5f;
    public float duration = 3f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.ApplySlow(slowMultiplier, duration);
            }
        }
    }
}
