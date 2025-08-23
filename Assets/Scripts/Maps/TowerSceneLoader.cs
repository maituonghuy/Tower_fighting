using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TowerSceneLoader : MonoBehaviour
{
    public Image player1Image;
    public Image[] player1Skills;

    public Image player2Image;
    public Image[] player2Skills;

    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    void Start()
    {
        var data = CharacterSelectionData.Instance;

        // ⬇️ THÊM: Instantiate player thật vào scene
        GameObject p1 = Instantiate(data.player1Character.characterPrefab, player1SpawnPoint.position, Quaternion.identity);
        GameObject p2 = Instantiate(data.player2Character.characterPrefab, player2SpawnPoint.position, Quaternion.identity);

        // Gán PlayerType để phân biệt điều khiển
        p1.GetComponent<PlayerController>().SetPlayerType(PlayerType.Player1);
        p2.GetComponent<PlayerController>().SetPlayerType(PlayerType.Player2);

        p1.GetComponent<PlayerController>().itemUI = GameObject.Find("Player1_UI").GetComponent<UIController>();
        p2.GetComponent<PlayerController>().itemUI = GameObject.Find("Player2_UI").GetComponent<UIController>();

        data.player1Instance = p1;
        data.player2Instance = p2;

        DontDestroyOnLoad(p1);
        DontDestroyOnLoad(p2);

        //Invoke(nameof(GoToScene2), 10f);
    }

    public void GoToScene2()
    {
        SceneManager.LoadScene("EndGameScene");
    }
}