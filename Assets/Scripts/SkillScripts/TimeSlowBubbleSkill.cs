using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TowerGame/Skill/TimeSlowBubble")]
public class TimeSlowBubbleSkill : Skill
{
    [Header("Bubble Settings")]
    [SerializeField] private float bubbleDuration = 3f; // Increased to 3 seconds
    [SerializeField] private float bubbleRadius = 7f; // Increased radius
    [SerializeField] private float slowMultiplier = 0.5f; // More severe slowdown (70% speed reduction)
    [SerializeField] private GameObject bubblePrefab;
    
    [Header("Visual Effects")]
    [SerializeField] private LayerMask affectedLayers = -1; // Which layers are affected
    [SerializeField] private bool debugMode = false; // Enable debug logs
    
    private Dictionary<GameObject, BubbleInstance> activeBubbles = new Dictionary<GameObject, BubbleInstance>();

    public override void Activate(GameObject player, SkillExecutor executor)
    {
        // Check if player already has an active bubble
        if (activeBubbles.ContainsKey(player))
        {
            Debug.Log($"{player.name} already has an active Time-Slow Bubble!");
            return;
        }

        Debug.Log($"{player.name} activated Time-Slow Bubble!");

        // Create bubble instance
        BubbleInstance bubbleInstance = new BubbleInstance();
        bubbleInstance.caster = player;
        bubbleInstance.affectedPlayers = new List<PlayerController>();
        
        // Create visual bubble if prefab exists
        if (bubblePrefab != null)
        {
            bubbleInstance.bubbleVisual = Object.Instantiate(bubblePrefab, player.transform.position, Quaternion.identity);
            bubbleInstance.bubbleVisual.transform.localScale = Vector3.one * (bubbleRadius * 2f);
            
            // Make the bubble follow the player
            bubbleInstance.bubbleVisual.transform.SetParent(player.transform);
        }

        // Store the bubble instance
        activeBubbles[player] = bubbleInstance;

        // Start the bubble effect coroutine
        executor.StartCoroutine(BubbleEffectCoroutine(bubbleInstance));

        // Start cooldown
        executor.StartCooldown(this);
    }

    public override void Cancel(GameObject player, SkillExecutor executor)
    {
        if (activeBubbles.ContainsKey(player))
        {
            EndBubbleEffect(activeBubbles[player]);
            Debug.Log($"{player.name} cancelled Time-Slow Bubble!");
        }
    }

    private IEnumerator BubbleEffectCoroutine(BubbleInstance bubbleInstance)
    {
        float elapsedTime = 0f;

        while (elapsedTime < bubbleDuration && bubbleInstance.caster != null)
        {
            // Update bubble effect every frame
            UpdateBubbleEffect(bubbleInstance);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // End bubble effect
        EndBubbleEffect(bubbleInstance);
    }

    private void UpdateBubbleEffect(BubbleInstance bubbleInstance)
    {
        if (bubbleInstance.caster == null) return;

        Vector3 bubbleCenter = bubbleInstance.caster.transform.position;
        
        // Find all colliders in bubble range
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(bubbleCenter, bubbleRadius, affectedLayers);
        
        if (debugMode)
        {
            Debug.Log($"Bubble at {bubbleCenter}, found {collidersInRange.Length} colliders in range of {bubbleRadius} units");
        }
        
        // Track which players are currently in range
        List<PlayerController> playersInRange = new List<PlayerController>();
        
        foreach (Collider2D collider in collidersInRange)
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player != null && player.gameObject != bubbleInstance.caster)
            {
                playersInRange.Add(player);
                
                if (debugMode)
                {
                    float distance = Vector3.Distance(bubbleCenter, player.transform.position);
                    Debug.Log($"Found player {player.GetPlayerType()} at distance {distance}");
                }
                
                // Apply slow effect if not already affected
                if (!bubbleInstance.affectedPlayers.Contains(player))
                {
                    ApplySlowEffect(player);
                    bubbleInstance.affectedPlayers.Add(player);
                    Debug.Log($"{player.GetPlayerType()} entered Time-Slow Bubble and was slowed!");
                }
            }
        }
        
        // Remove slow effect from players who left the bubble
        List<PlayerController> playersToRemove = new List<PlayerController>();
        foreach (PlayerController affectedPlayer in bubbleInstance.affectedPlayers)
        {
            if (!playersInRange.Contains(affectedPlayer))
            {
                RemoveSlowEffect(affectedPlayer);
                playersToRemove.Add(affectedPlayer);
                Debug.Log($"{affectedPlayer.GetPlayerType()} left Time-Slow Bubble and speed restored!");
            }
        }
        
        // Clean up the affected players list
        foreach (PlayerController playerToRemove in playersToRemove)
        {
            bubbleInstance.affectedPlayers.Remove(playerToRemove);
        }
    }

    private void ApplySlowEffect(PlayerController player)
    {
        // Apply slow effect through PlayerController
        if (player != null)
        {
            Debug.Log($"Applying slow effect to {player.GetPlayerType()} with multiplier {slowMultiplier}");
            player.ApplySpeedModifier("TimeSlowBubble", slowMultiplier);
        }
    }

    private void RemoveSlowEffect(PlayerController player)
    {
        // Remove slow effect through PlayerController
        if (player != null)
        {
            Debug.Log($"Removing slow effect from {player.GetPlayerType()}");
            player.RemoveSpeedModifier("TimeSlowBubble");
        }
    }

    private void EndBubbleEffect(BubbleInstance bubbleInstance)
    {
        // Remove slow effects from all affected players
        foreach (PlayerController affectedPlayer in bubbleInstance.affectedPlayers)
        {
            if (affectedPlayer != null)
            {
                RemoveSlowEffect(affectedPlayer);
            }
        }

        // Destroy visual bubble
        if (bubbleInstance.bubbleVisual != null)
        {
            Object.Destroy(bubbleInstance.bubbleVisual);
        }

        // Remove from active bubbles
        if (activeBubbles.ContainsKey(bubbleInstance.caster))
        {
            activeBubbles.Remove(bubbleInstance.caster);
        }

        Debug.Log("Time-Slow Bubble effect ended!");
    }

    // Helper class to track bubble instances
    private class BubbleInstance
    {
        public GameObject caster;
        public GameObject bubbleVisual;
        public List<PlayerController> affectedPlayers;
    }

    // Gizmo for debugging in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, bubbleRadius);
    }
}
