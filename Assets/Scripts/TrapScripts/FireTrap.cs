using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField]
    public float burnDamage = 3f;
    [SerializeField]
    public float burnDuration = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.ApplyBurn(burnDamage, burnDuration);
            }
        }
    }
}
