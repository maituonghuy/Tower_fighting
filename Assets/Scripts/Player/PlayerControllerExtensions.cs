using UnityEngine;
using System.Collections.Generic;

// Simple extension methods for PlayerController
public static class PlayerControllerExtensions
{
    private static Dictionary<PlayerController, Dictionary<string, float>> speedModifiers = 
        new Dictionary<PlayerController, Dictionary<string, float>>();

    public static void ApplySpeedModifier(this PlayerController player, string source, float multiplier)
    {
        if (!speedModifiers.ContainsKey(player))
        {
            speedModifiers[player] = new Dictionary<string, float>();
        }

        // Store the modifier
        speedModifiers[player][source] = multiplier;
        
        // Recalculate speed
        UpdatePlayerSpeed(player);
        
        Debug.Log($"Applied speed modifier '{source}' to {player.GetPlayerType()}: {multiplier}x");
    }

    public static void RemoveSpeedModifier(this PlayerController player, string source)
    {
        if (speedModifiers.ContainsKey(player) && speedModifiers[player].ContainsKey(source))
        {
            speedModifiers[player].Remove(source);
            UpdatePlayerSpeed(player);
            Debug.Log($"Removed speed modifier '{source}' from {player.GetPlayerType()}");
        }
    }

    private static void UpdatePlayerSpeed(PlayerController player)
    {
        float totalMultiplier = 1f;
        
        if (speedModifiers.ContainsKey(player))
        {
            foreach (var modifier in speedModifiers[player])
            {
                totalMultiplier *= modifier.Value;
            }
        }

        // Apply the total multiplier to the player's movement
        // Note: You'll need to modify PlayerController to have a SetSpeedMultiplier method
        // or access the moveSpeed field directly
    }

    public static void ClearAllSpeedModifiers(this PlayerController player)
    {
        if (speedModifiers.ContainsKey(player))
        {
            speedModifiers[player].Clear();
            UpdatePlayerSpeed(player);
        }
    }
}
