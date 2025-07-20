using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public TrapData trapData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player == null) return;

        if (trapData.effectType == TrapEffectType.Damage)
        {
            player.TakeDamage(trapData.value);
            Debug.Log("Trap triggered: " + trapData.trapName);
        }
    }
}
