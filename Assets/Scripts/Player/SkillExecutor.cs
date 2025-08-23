using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    [Header("Skill Assets")]
    [SerializeField] private List<Skill> skills;
    private Dictionary<Skill, float> cooldownTimer = new();
    private GameObject player;
    private PlayerController playerController;
    

    void Start()
    {
        // Assign cooldown for each skill to dictionary
        foreach (Skill skill in skills)
        {
            cooldownTimer[skill] = 0f;
        }
        // Assign the GameObject this script is attached to as the player
        player = this.gameObject;
        
        // Get the PlayerController component
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("SkillExecutor: PlayerController component not found!");
        }
        
        Debug.Log("Init skill cooldown successfully");
    }

    void Update()
    {
        // Check if we have skills available
        if (skills == null || skills.Count == 0) return;
        
        // Check if playerController is available
        if (playerController == null) return;

        // Get current player type from PlayerController
        PlayerType currentPlayerType = playerController.GetPlayerType();

        // Handle input based on player type
        bool activatePressed = false;
        bool cancelPressed = false;

        if (currentPlayerType == PlayerType.Player1)
        {
            // Player Type 1: F to activate, G to cancel
            activatePressed = UnityEngine.InputSystem.Keyboard.current.fKey.wasReleasedThisFrame;
            cancelPressed = UnityEngine.InputSystem.Keyboard.current.gKey.wasReleasedThisFrame;
        }
        else if (currentPlayerType == PlayerType.Player2)
        {
            // Player Type 2: I to activate, P to cancel
            activatePressed = UnityEngine.InputSystem.Keyboard.current.iKey.wasReleasedThisFrame;
            cancelPressed = UnityEngine.InputSystem.Keyboard.current.pKey.wasReleasedThisFrame;
        }

        // Activate special skill
        if (activatePressed && skills.Count > 0 && !isOnCoolDown(skills[0]))
        {
            Debug.Log($"Player {currentPlayerType}: Pre Activate special skill");
            skills[0].Activate(player, this);
            Debug.Log($"Player {currentPlayerType}: Activate special skill");
        }
        
        // Cancel special skill
        if (cancelPressed && skills.Count > 0)
        {
            Debug.Log($"Player {currentPlayerType}: Cancel special skill");
            skills[0].Cancel(player, this);
        }
    }
    
    public void SetPlayerType(PlayerType type)
    {
        if (playerController != null)
        {
            playerController.SetPlayerType(type);
            Debug.Log($"SkillExecutor: Player type set to {type}");
        }
        else
        {
            Debug.LogError("SkillExecutor: PlayerController not found, cannot set player type");
        }
    }

    public PlayerType GetPlayerType()
    {
        if (playerController != null)
        {
            return playerController.GetPlayerType();
        }
        Debug.LogError("SkillExecutor: PlayerController not found, returning default Player1");
        return PlayerType.Player1;
    }

    public void StartCooldown(Skill skill)
    {
        cooldownTimer[skill] = Time.time + skill.cooldown;
    }

    public bool isOnCoolDown(Skill skill)
    {
        return cooldownTimer.ContainsKey(skill) && Time.time <= cooldownTimer[skill];
    }
}
