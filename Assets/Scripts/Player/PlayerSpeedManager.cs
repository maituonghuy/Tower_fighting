using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpeedModifier
{
    public string source;
    public float multiplier;
    public float duration;
    public float startTime;

    public SpeedModifier(string source, float multiplier, float duration = -1f)
    {
        this.source = source;
        this.multiplier = multiplier;
        this.duration = duration;
        this.startTime = Time.time;
    }

    public bool IsExpired()
    {
        return duration > 0 && (Time.time - startTime) >= duration;
    }
}

public class PlayerSpeedManager : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float baseSpeed = 5f;
    private float currentSpeed;
    
    private List<SpeedModifier> activeModifiers = new List<SpeedModifier>();
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        // Remove expired modifiers
        activeModifiers.RemoveAll(modifier => modifier.IsExpired());
        
        // Recalculate speed
        RecalculateSpeed();
    }

    public void ApplySpeedModifier(string source, float multiplier, float duration = -1f)
    {
        // Remove existing modifier from the same source
        RemoveSpeedModifier(source);
        
        // Add new modifier
        activeModifiers.Add(new SpeedModifier(source, multiplier, duration));
        
        // Recalculate speed
        RecalculateSpeed();
        
        Debug.Log($"Applied speed modifier '{source}': {multiplier}x for {(duration > 0 ? duration.ToString() + "s" : "permanent")}");
    }

    public void RemoveSpeedModifier(string source)
    {
        int removedCount = activeModifiers.RemoveAll(modifier => modifier.source == source);
        if (removedCount > 0)
        {
            RecalculateSpeed();
            Debug.Log($"Removed speed modifier '{source}'");
        }
    }

    private void RecalculateSpeed()
    {
        float newSpeed = baseSpeed;
        
        // Apply all active modifiers
        foreach (SpeedModifier modifier in activeModifiers)
        {
            newSpeed *= modifier.multiplier;
        }
        
        currentSpeed = newSpeed;
        
        // Update PlayerController speed if available
        if (playerController != null)
        {
            playerController.UpdateMoveSpeed(currentSpeed);
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public void SetBaseSpeed(float newBaseSpeed)
    {
        baseSpeed = newBaseSpeed;
        RecalculateSpeed();
    }

    public List<SpeedModifier> GetActiveModifiers()
    {
        return new List<SpeedModifier>(activeModifiers);
    }
}
