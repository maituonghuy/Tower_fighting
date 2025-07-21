using UnityEngine;

public class Scene2Load : MonoBehaviour
{
    public Transform newSpawnPoint1;
    public Transform newSpawnPoint2;

    void Start()
    {
        var data = CharacterSelectionData.Instance;

        if (data.player1Instance != null)
        {
            data.player1Instance.transform.position = newSpawnPoint1.position;
        }

        if (data.player2Instance != null)
        {
            data.player2Instance.transform.position = newSpawnPoint2.position;
        }
    }
}
