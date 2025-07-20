using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    [Header("Skill Assets")]
    [SerializeField] private List<Skill> skills;
    private Dictionary<Skill, float> cooldownTimer = new();
    private GameObject player;
    

    void Start()
    {
        // Assign cooldown for each skill to dictionary
        foreach (Skill skill in skills)
        {
            cooldownTimer[skill] = 0f;
        }
        // Assign the GameObject this script is attached to as the player
        player = this.gameObject;
        
        Debug.Log("Init skill cooldown successfully");
    }

    void Update()
    {
        // Check if we have skills available
        if (skills == null || skills.Count == 0) return;

        //Special skill (Time-Slow Bubble or Fire Stream - depends on what's assigned)
        if (UnityEngine.InputSystem.Keyboard.current.eKey.wasReleasedThisFrame && skills.Count > 0 && !isOnCoolDown(skills[0]))
        {
            Debug.Log("Pre Activate special skill");
            skills[0].Activate(player, this);
            Debug.Log("Activate special skill");
        }
        
        //Cancel special skill
        if (UnityEngine.InputSystem.Keyboard.current.qKey.wasReleasedThisFrame && skills.Count > 0)
        {
            Debug.Log("Cancel special skill");
            skills[0].Cancel(player, this);
        }

        // Secondary skill (Dash, Teleport, etc. - depends on what's assigned)
        if (UnityEngine.InputSystem.Keyboard.current.cKey.wasPressedThisFrame && skills.Count > 1 && !isOnCoolDown(skills[1]))
        {
            Debug.Log("Pre Activate secondary skill");
            skills[1].Activate(player, this);
            Debug.Log("Activate secondary skill");
        }

        // Third skill slot (if needed)
        if (UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame && skills.Count > 2 && !isOnCoolDown(skills[2]))
        {
            Debug.Log("Pre Activate third skill");
            skills[2].Activate(player, this);
            Debug.Log("Activate third skill");
        }
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
