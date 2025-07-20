using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject mapPrefab;
    public GameObject playerPrefab;

    void Start()
    {
        var map = Instantiate(mapPrefab);
        Transform spawn1 = map.transform.Find("SpawnPoints/SpawnPoint1");
        Transform spawn2 = map.transform.Find("SpawnPoints/SpawnPoint2");

        GameObject p1 = Instantiate(playerPrefab, spawn1.position, Quaternion.identity);
        GameObject p2 = Instantiate(playerPrefab, spawn2.position, Quaternion.identity);

        p1.GetComponent<PlayerController>().SetPlayerType(PlayerType.Player1);
        p2.GetComponent<PlayerController>().SetPlayerType(PlayerType.Player2);


        var anchorCam = map.transform.Find("AnchorCamera").GetComponent<PKCameraController>();
        anchorCam.player1 = p1.transform;
        anchorCam.player2 = p2.transform;
    }
}
