using UnityEngine;

[CreateAssetMenu(menuName = "TowerGame/Skill/Teleport")]
public class TeleportScripts : Skill
{
    [SerializeField]
    public GameObject teleportMarkPrefab;
    private GameObject currentTeleportMark;
    private bool checkMarkActive = false;
    public override void Activate(GameObject player, SkillExecutor executor)
    {
        //Create a mark as teleport destination
        if (checkMarkActive == false)
        {
            Debug.Log("Pre mark successfully");
            currentTeleportMark = Instantiate(teleportMarkPrefab, player.transform.position, Quaternion.identity);
            currentTeleportMark.SetActive(true);
            checkMarkActive = true;
            Debug.Log("Generate mark successfully");
        }
        else if (checkMarkActive == true)//Check if mark is gernerated
        { try
            {
                Debug.Log($"Pre teleport {checkMarkActive}");
                Vector3 teleportPosition = currentTeleportMark.transform.position;
                player.transform.position = teleportPosition;
                Destroy(currentTeleportMark);
                checkMarkActive = false;
                executor.StartCooldown(this);
                Debug.Log("Teleport successfully");
            }
            catch (System.Exception e)
            {
                checkMarkActive = false;
                executor.StartCooldown(this);
                Debug.LogError($"Teleport failed: {e.Message}");
                
            }
            
        }
        //executor play animation

        //executor update cooldown

    }
    // Update is called once per frame
    void Update()
    {
    }
    public override void Cancel(GameObject player, SkillExecutor executor)
    {
        if (checkMarkActive)
        {
            Destroy(currentTeleportMark);
            checkMarkActive = false;
            Debug.Log("Teleport mark cancelled.");

        }
        else
        {
            Debug.Log("No teleport mark to cancel.");
        }
    }
}
