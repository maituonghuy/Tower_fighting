using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "TowerGame/Skill/WindGust")]
public class WindGustSkill : Skill
{
    [Header("Wind Gust Settings")]
    [SerializeField] private float gustDuration = 0.3f; // Short burst duration
    [SerializeField] private float gustRange = 1.5f; // Short range
    [SerializeField] private float gustWidth = 2f; // Height of wind area
    [SerializeField] private float knockbackForce = 500f; // Strong knockback
    [SerializeField] private float upwardForce = 200f; // Slight upward push
    [SerializeField] private float windLifetime = 0.5f; // How long wind visual lasts
    
    [Header("Movement Settings")]
    [SerializeField] private float travelDistance = 3f; // Distance the wind travels
    [SerializeField] private float travelSpeed = 5f; // Speed of wind movement
    
    [Header("Positioning")]
    [SerializeField] private Vector2 windOriginOffset = new Vector2(0.2f, 0f); // X: side offset, Y: height offset
    
    [Header("Visual Effects")]
    [SerializeField] private GameObject windGustPrefab;
    [SerializeField] private LayerMask affectedLayers = -1;
    [SerializeField] private bool debugMode = true;
    [SerializeField] private bool showRangeInScene = true;
    
    public override void Activate(GameObject player, SkillExecutor executor)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found on player!");
            return;
        }

        Debug.Log($"{player.name} activated Wind Gust!");

        // Get facing direction (locked when skill is triggered)
        float facingDirection = Mathf.Sign(player.transform.localScale.x);
        
        // Create moving wind gust
        executor.StartCoroutine(MovingWindGustCoroutine(player, facingDirection));

        // Start cooldown
        executor.StartCooldown(this);
    }

    public override void Cancel(GameObject player, SkillExecutor executor)
    {
        // Wind gust is instant, nothing to cancel
        Debug.Log($"{player.name} - Wind Gust cannot be cancelled (instant effect)!");
    }

    private IEnumerator MovingWindGustCoroutine(GameObject caster, float facingDirection)
    {
        // Calculate starting position
        Vector3 sideOffset = new Vector3(windOriginOffset.x * facingDirection, windOriginOffset.y, 0f);
        Vector3 startPosition = caster.transform.position + sideOffset;
        Vector3 endPosition = startPosition + Vector3.right * facingDirection * travelDistance;
        
        // Create visual wind gust if prefab exists
        GameObject windVisual = null;
        if (windGustPrefab != null)
        {
            windVisual = Object.Instantiate(windGustPrefab, startPosition, Quaternion.identity);
            
            // Scale and orient the wind gust
            windVisual.transform.localScale = new Vector3(
                gustRange * facingDirection, 
                gustWidth, 
                1f
            );
        }
        
        float elapsedTime = 0f;
        float travelTime = travelDistance / travelSpeed;
        
        // Move wind gust and apply effects
        while (elapsedTime < travelTime)
        {
            float progress = elapsedTime / travelTime;
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, progress);
            
            // Update visual position
            if (windVisual != null)
            {
                windVisual.transform.position = currentPosition;
            }
            
            // Apply wind effect at current position
            ApplyWindGustAtPosition(caster, currentPosition, facingDirection);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Clean up visual
        if (windVisual != null)
        {
            Object.Destroy(windVisual);
        }

        Debug.Log("Moving Wind Gust effect ended!");
    }

    private void ApplyWindGust(GameObject caster, float facingDirection)
    {
        if (caster == null) return;

        // Calculate wind gust area position
        Vector3 sideOffset = new Vector3(windOriginOffset.x * facingDirection, windOriginOffset.y, 0f);
        Vector3 forwardOffset = Vector3.right * facingDirection * (gustRange / 2f);
        Vector3 gustCenter = caster.transform.position + sideOffset + forwardOffset;
        
        // Create a box area for the wind gust
        Vector2 boxSize = new Vector2(gustRange, gustWidth);
        Collider2D[] collidersInRange = Physics2D.OverlapBoxAll(gustCenter, boxSize, 0f, affectedLayers);
        
        if (debugMode)
        {
            Debug.Log($"Wind gust at {gustCenter}, found {collidersInRange.Length} colliders in range");
        }
        
        foreach (Collider2D collider in collidersInRange)
        {
            PlayerController targetPlayer = collider.GetComponent<PlayerController>();
            if (targetPlayer != null && targetPlayer.gameObject != caster)
            {
                // Apply wind knockback (no damage)
                ApplyWindKnockback(targetPlayer, facingDirection);
                
                if (debugMode)
                {
                    Debug.Log($"Wind gust pushed {targetPlayer.GetPlayerType()}");
                }
            }
        }
    }

    private void ApplyWindGustAtPosition(GameObject caster, Vector3 gustPosition, float facingDirection)
    {
        if (caster == null) return;

        // Create a box area for the wind gust at the given position
        Vector2 boxSize = new Vector2(gustRange, gustWidth);
        Collider2D[] collidersInRange = Physics2D.OverlapBoxAll(gustPosition, boxSize, 0f, affectedLayers);
        
        if (debugMode)
        {
            Debug.Log($"Wind gust at {gustPosition}, found {collidersInRange.Length} colliders in range");
        }
        
        foreach (Collider2D collider in collidersInRange)
        {
            PlayerController targetPlayer = collider.GetComponent<PlayerController>();
            if (targetPlayer != null && targetPlayer.gameObject != caster)
            {
                // Apply wind knockback (no damage)
                ApplyWindKnockback(targetPlayer, facingDirection);
                
                if (debugMode)
                {
                    Debug.Log($"Wind gust pushed {targetPlayer.GetPlayerType()}");
                }
            }
        }
    }

    private void ApplyWindKnockback(PlayerController player, float direction)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Create wind force: horizontal + slight upward
            Vector2 windDirection = new Vector2(direction, 0.4f).normalized; // 0.4f = slight upward angle
            Vector2 totalForce = windDirection * knockbackForce + Vector2.up * upwardForce;
            
            // Apply force
            rb.AddForce(totalForce, ForceMode2D.Impulse);
            
            if (debugMode)
            {
                Debug.Log($"Applied wind knockback to {player.GetPlayerType()} with force {totalForce}");
            }
        }
    }

    // Gizmo for debugging in Scene view
    private void OnDrawGizmosSelected()
    {
        if (!showRangeInScene) return;
        
        // Draw wind gust range for both directions
        DrawWindGustRange(1f);  // Right direction
        DrawWindGustRange(-1f); // Left direction
        
        // Draw origin point
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(windOriginOffset, 0.1f);
    }
    
    private void DrawWindGustRange(float facingDirection)
    {
        Gizmos.color = facingDirection > 0 ? Color.cyan : Color.blue;
        
        // Calculate wind gust area
        Vector3 sideOffset = new Vector3(windOriginOffset.x * facingDirection, windOriginOffset.y, 0f);
        Vector3 forwardOffset = Vector3.right * facingDirection * (gustRange / 2f);
        Vector3 center = sideOffset + forwardOffset;
        
        // Draw wind area
        Gizmos.DrawWireCube(center, new Vector3(gustRange, gustWidth, 0.1f));
        
        // Draw range line
        Vector3 startPos = sideOffset;
        Vector3 endPos = sideOffset + Vector3.right * facingDirection * gustRange;
        Gizmos.DrawLine(startPos, endPos);
        
        // Draw wind direction arrows
        Vector3 arrowStart = center;
        Vector3 arrowEnd = arrowStart + Vector3.right * facingDirection * 0.5f + Vector3.up * 0.2f;
        Gizmos.DrawLine(arrowStart, arrowEnd);
        
        // Draw range text (if in editor)
        #if UNITY_EDITOR
        UnityEditor.Handles.color = Gizmos.color;
        UnityEditor.Handles.Label(endPos + Vector3.up * 0.3f, $"Wind: {gustRange}u");
        #endif
    }
}
