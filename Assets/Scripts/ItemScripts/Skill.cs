using UnityEngine;

[CreateAssetMenu(menuName = "TowerGame/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public float cooldown;

    public virtual void Activate(PlayerController player)
    {
        Debug.Log($"{player.GetPlayerType()} dùng skill: {skillName}");
    }
}
