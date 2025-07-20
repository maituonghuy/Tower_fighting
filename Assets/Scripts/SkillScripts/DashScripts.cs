using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "TowerGame/Skill/Dash")]
public class DashScript : Skill
{


    void Start()
    {
        // Initialization if needed

    }
    public override void Activate(GameObject player, SkillExecutor executor)
    {
    

        executor.StartCooldown(this);
    }
}
