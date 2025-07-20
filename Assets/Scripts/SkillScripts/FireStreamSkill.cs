using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TowerGame/Skill/FireStream")]
public class FireStreamSkill : Skill
{
    [Header("Fire Stream Settings")]
    [SerializeField] private float streamDuration = 3f;
    [SerializeField] private float streamRange = 4f; // 3-4 player widths
    [SerializeField] private float streamWidth = 1.5f;
    [SerializeField] private float damagePerTick = 5f;
    [SerializeField] private float tickInterval = 0.2f;
    [SerializeField] private float knockbackForce = 200f;
    
    [Header("Positioning")]
    [SerializeField] private Vector2 fireOriginOffset = new Vector2(0.3f, 0.2f); // X: side offset, Y: height offset
    
    [Header("Visual Effects")]
    [SerializeField] private GameObject fireStreamPrefab;
    [SerializeField] private LayerMask affectedLayers = -1;
    [SerializeField] private bool debugMode = true;
    
    private Dictionary<GameObject, FireStreamInstance> activeStreams = new Dictionary<GameObject, FireStreamInstance>();

    public override void Activate(GameObject player, SkillExecutor executor)
    {
        // Check if player already has an active fire stream
        if (activeStreams.ContainsKey(player))
        {
            Debug.Log($"{player.name} already has an active Fire Stream!");
            return;
        }

        PlayerController playerController = player.GetComponent<PlayerController>();
        
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found on player!");
            return;
        }

        Debug.Log($"{player.name} activated Fire Stream!");

        // Create fire stream instance
        FireStreamInstance streamInstance = new FireStreamInstance();
        streamInstance.caster = player;
        streamInstance.playerController = playerController;
        streamInstance.affectedPlayers = new List<PlayerController>();
        streamInstance.lastDamageTimes = new Dictionary<PlayerController, float>();
        
        // Get facing direction
        streamInstance.facingDirection = Mathf.Sign(player.transform.localScale.x);
        
        // Create visual fire stream if prefab exists
        if (fireStreamPrefab != null)
        {
            // Calculate fire origin position (side of player + forward offset)
            Vector3 sideOffset = new Vector3(fireOriginOffset.x * streamInstance.facingDirection, fireOriginOffset.y, 0f);
            Vector3 forwardOffset = Vector3.right * streamInstance.facingDirection * (streamRange / 2f);
            Vector3 streamPosition = player.transform.position + sideOffset + forwardOffset;
            
            streamInstance.fireVisual = Object.Instantiate(fireStreamPrefab, streamPosition, Quaternion.identity);
            
            // Scale the fire stream to match range and width
            streamInstance.fireVisual.transform.localScale = new Vector3(streamRange, streamWidth, 1f);
            
            // Flip the fire stream based on facing direction
            if (streamInstance.facingDirection < 0)
            {
                Vector3 scale = streamInstance.fireVisual.transform.localScale;
                scale.x *= -1;
                streamInstance.fireVisual.transform.localScale = scale;
            }
            
            // Make the fire stream follow the player
            streamInstance.fireVisual.transform.SetParent(player.transform);
        }

        // Disable dashing during fire stream
        streamInstance.playerController.SetCanDash(false);

        // Store the stream instance
        activeStreams[player] = streamInstance;

        // Start the fire stream effect coroutine
        executor.StartCoroutine(FireStreamCoroutine(streamInstance));

        // Start cooldown
        executor.StartCooldown(this);
    }

    public override void Cancel(GameObject player, SkillExecutor executor)
    {
        if (activeStreams.ContainsKey(player))
        {
            EndFireStream(activeStreams[player]);
            Debug.Log($"{player.name} cancelled Fire Stream!");
        }
    }

    private IEnumerator FireStreamCoroutine(FireStreamInstance streamInstance)
    {
        float elapsedTime = 0f;
        float nextDamageTime = 0f;

        while (elapsedTime < streamDuration && streamInstance.caster != null)
        {
            // Update fire stream effect
            UpdateFireStream(streamInstance);
            
            // Apply damage at intervals
            if (elapsedTime >= nextDamageTime)
            {
                ApplyFireDamage(streamInstance);
                nextDamageTime += tickInterval;
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // End fire stream effect
        EndFireStream(streamInstance);
    }

    private void UpdateFireStream(FireStreamInstance streamInstance)
    {
        if (streamInstance.caster == null) return;

        // Update facing direction (player can turn while firing)
        float currentFacing = Mathf.Sign(streamInstance.caster.transform.localScale.x);
        if (currentFacing != streamInstance.facingDirection)
        {
            streamInstance.facingDirection = currentFacing;
            UpdateFireStreamVisual(streamInstance);
        }

        // Update fire stream position to follow player
        if (streamInstance.fireVisual != null)
        {
            // Calculate fire origin position (side of player + forward offset)
            Vector3 sideOffset = new Vector3(fireOriginOffset.x * streamInstance.facingDirection, fireOriginOffset.y, 0f);
            Vector3 forwardOffset = Vector3.right * streamInstance.facingDirection * (streamRange / 2f);
            Vector3 streamPosition = streamInstance.caster.transform.position + sideOffset + forwardOffset;
            
            streamInstance.fireVisual.transform.position = streamPosition;
        }
    }

    private void UpdateFireStreamVisual(FireStreamInstance streamInstance)
    {
        if (streamInstance.fireVisual == null) return;

        // Update visual direction
        Vector3 scale = streamInstance.fireVisual.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * streamInstance.facingDirection;
        streamInstance.fireVisual.transform.localScale = scale;
    }

    private void ApplyFireDamage(FireStreamInstance streamInstance)
    {
        if (streamInstance.caster == null) return;

        // Calculate fire damage area position (same as visual)
        Vector3 sideOffset = new Vector3(fireOriginOffset.x * streamInstance.facingDirection, fireOriginOffset.y, 0f);
        Vector3 forwardOffset = Vector3.right * streamInstance.facingDirection * (streamRange / 2f);
        Vector3 streamCenter = streamInstance.caster.transform.position + sideOffset + forwardOffset;
        
        // Create a box area for the fire stream
        Vector2 boxSize = new Vector2(streamRange, streamWidth);
        Collider2D[] collidersInRange = Physics2D.OverlapBoxAll(streamCenter, boxSize, 0f, affectedLayers);
        
        if (debugMode)
        {
            Debug.Log($"Fire stream at {streamCenter}, found {collidersInRange.Length} colliders in range");
        }
        
        foreach (Collider2D collider in collidersInRange)
        {
            PlayerController targetPlayer = collider.GetComponent<PlayerController>();
            if (targetPlayer != null && targetPlayer.gameObject != streamInstance.caster)
            {
                // Check if enough time has passed since last damage to this player
                if (!streamInstance.lastDamageTimes.ContainsKey(targetPlayer) || 
                    Time.time - streamInstance.lastDamageTimes[targetPlayer] >= tickInterval)
                {
                    // Apply damage
                    targetPlayer.ApplyDamage(damagePerTick);
                    
                    // Apply knockback
                    ApplyKnockback(targetPlayer, streamInstance.facingDirection);
                    
                    // Update last damage time
                    streamInstance.lastDamageTimes[targetPlayer] = Time.time;
                    
                    if (debugMode)
                    {
                        Debug.Log($"Fire stream damaged {targetPlayer.GetPlayerType()} for {damagePerTick} damage");
                    }
                }
            }
        }
    }

    private void ApplyKnockback(PlayerController player, float direction)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 knockbackDirection = new Vector2(direction, 0.3f).normalized; // Slight upward angle
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            
            if (debugMode)
            {
                Debug.Log($"Applied knockback to {player.GetPlayerType()} in direction {knockbackDirection}");
            }
        }
    }

    private void EndFireStream(FireStreamInstance streamInstance)
    {
        // Re-enable dashing
        if (streamInstance.playerController != null)
        {
            streamInstance.playerController.SetCanDash(true);
        }

        // Destroy visual fire stream
        if (streamInstance.fireVisual != null)
        {
            Object.Destroy(streamInstance.fireVisual);
        }

        // Remove from active streams
        if (activeStreams.ContainsKey(streamInstance.caster))
        {
            activeStreams.Remove(streamInstance.caster);
        }

        Debug.Log("Fire Stream effect ended!");
    }

    // Helper class to track fire stream instances
    private class FireStreamInstance
    {
        public GameObject caster;
        public PlayerController playerController;
        public GameObject fireVisual;
        public float facingDirection;
        public List<PlayerController> affectedPlayers;
        public Dictionary<PlayerController, float> lastDamageTimes;
    }

    // Gizmo for debugging in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Show fire stream area with side offset
        Vector3 sideOffset = new Vector3(fireOriginOffset.x, fireOriginOffset.y, 0f);
        Vector3 forwardOffset = Vector3.right * (streamRange / 2f);
        Vector3 center = sideOffset + forwardOffset;
        Gizmos.DrawWireCube(center, new Vector3(streamRange, streamWidth, 0.1f));
    }
}
