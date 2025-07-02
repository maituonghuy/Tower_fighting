using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    public TrapData trapData;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player == null) return;

        if (trapData.effectType == TrapEffectType.Slow)
        {
            player.ApplySlow(trapData.value, trapData.duration);
            Debug.Log("Trap triggered: " + trapData.trapName);
        }
    }
}
