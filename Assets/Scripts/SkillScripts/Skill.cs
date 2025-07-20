using UnityEngine;

[CreateAssetMenu(menuName = "TowerGame/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public float cooldown;
    public Sprite icon;
    public Animation animation;


    public virtual void Activate(GameObject player, SkillExecutor executor)
    {
        Debug.Log($"{player} dùng skill: {skillName}");
    }
    public virtual void Cancel(GameObject player, SkillExecutor executor)
    {
        Debug.Log($"{player} hủy skill: {skillName}");
    }
}
