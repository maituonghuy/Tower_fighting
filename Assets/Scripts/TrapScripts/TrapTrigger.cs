using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public TrapData trapData;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ApplyTrapEffect(trapData);
            Debug.Log($"[Trap] {trapData.trapName} triggered on {player.GetPlayerType()}");
        }
    }
}
