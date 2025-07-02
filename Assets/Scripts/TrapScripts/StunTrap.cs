using UnityEngine;

public class StunTrap : MonoBehaviour
{
    public TrapData trapData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player == null) return;

        if (trapData.effectType == TrapEffectType.Stun)
        {
            player.ApplyStun(trapData.duration);
            Debug.Log("Trap triggered: " + trapData.trapName);
        }
    }
}
