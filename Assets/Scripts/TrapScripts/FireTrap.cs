using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public TrapData trapData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player == null) return;

        if (trapData.effectType == TrapEffectType.Burn)
        {
            player.ApplyBurn(trapData.value, trapData.duration);
            Debug.Log("Trap triggered: " + trapData.trapName);
        }
    }
}
