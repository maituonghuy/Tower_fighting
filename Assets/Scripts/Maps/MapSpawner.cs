using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [Header("Player Prefabs")]
    public GameObject playerPrefab;
    [Header("Buff Prefabs")]
    public GameObject[] buffPrefabs;
    [Header("Item Prefabs")]
    public GameObject[] itemPrefabs;
    [Header("Trap Prefabs")]
    public GameObject[] trapPrefabs;

    [Header("Player Spawn Points")]
    public Transform player1Spawn;
    public Transform player2Spawn;

    [Header("Buff Spawn Points")]
    public Transform[] buffSpawnPoints;
    [Header("Item Spawn Points")]
    public Transform[] itemSpawnPoints;
    [Header("Trap Spawn Points")]
    public Transform[] trapSpawnPoints;

    void Start()
    {
        //SpawnPlayer(PlayerType.Player1, player1Spawn);
        //SpawnPlayer(PlayerType.Player2, player2Spawn);

        SpawnBuffs();
        SpawnItems();
        SpawnTraps();
    }

    //void SpawnPlayer(PlayerType type, Transform spawnPoint)
    //{
    //    GameObject player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

    //    PlayerController controller = player.GetComponent<PlayerController>();
    //    if (controller != null)
    //    {
    //        typeof(PlayerController)
    //            .GetField("playerType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    //            ?.SetValue(controller, type);
    //    }
    //}

    void SpawnBuffs()
    {
        foreach (var point in buffSpawnPoints)
        {
            int rand = Random.Range(0, buffPrefabs.Length);
            Instantiate(buffPrefabs[rand], point.position, Quaternion.identity);
        }
    }

    void SpawnItems()
    {
        foreach (var point in itemSpawnPoints)
        {
            int rand = Random.Range(0, itemPrefabs.Length);
            Instantiate(itemPrefabs[rand], point.position, Quaternion.identity);
        }
    }

    void SpawnTraps()
    {
        foreach (var point in trapSpawnPoints)
        {
            int rand = Random.Range(0, trapPrefabs.Length);
            Instantiate(trapPrefabs[rand], point.position, Quaternion.identity);
        }
    }
}
