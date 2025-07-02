using UnityEngine;

public class Buff_Heal : MonoBehaviour
{
    public float healAmount = 30f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerHealth player = collider.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.ApplyHeal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
