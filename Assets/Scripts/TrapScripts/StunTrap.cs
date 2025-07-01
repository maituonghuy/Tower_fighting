using UnityEngine;

public class StunTrap : MonoBehaviour
{
    public float stunDuration = 1.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.ApplyStun(stunDuration);
            }
        }
    }
}
