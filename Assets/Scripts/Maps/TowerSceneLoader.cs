using UnityEngine;
using UnityEngine.UI;


public class TowerSceneLoader : MonoBehaviour
{
    public Image player1Image;
    public Image[] player1Skills;

    public Image player2Image;
    public Image[] player2Skills;

    public GameObject playerPrefab;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    void Start()
    {
        var data = CharacterSelectionData.Instance;

        // ⬇️ UI hiển thị hình (đoạn bạn đã có)
        player1Image.sprite = data.player1Character.characterSprite;
        //for (int i = 0; i < player1Skills.Length; i++)
        //{
        //    if (i < data.player1Character.skillSprites.Length)
        //        player1Skills[i].sprite = data.player1Character.skillSprites[i];
        //}

        //player2Image.sprite = data.player2Character.characterSprite;
        //for (int i = 0; i < player2Skills.Length; i++)
        //{
        //    if (i < data.player2Character.skillSprites.Length)
        //        player2Skills[i].sprite = data.player2Character.skillSprites[i];
        //}

        // ⬇️ THÊM: Instantiate player thật vào scene
        GameObject p1 = Instantiate(playerPrefab, player1SpawnPoint.position, Quaternion.identity);
        GameObject p2 = Instantiate(playerPrefab, player2SpawnPoint.position, Quaternion.identity);

        // Đổi sprite của nhân vật dựa trên nhân vật đã chọn
        p1.GetComponentInChildren<SpriteRenderer>().sprite = data.player1Character.characterSprite;
        p2.GetComponentInChildren<SpriteRenderer>().sprite = data.player2Character.characterSprite;

        // Gán PlayerType để phân biệt điều khiển
        p1.GetComponent<PlayerController>().SetPlayerType(PlayerType.Player1);
        p2.GetComponent<PlayerController>().SetPlayerType(PlayerType.Player2);

        p1.GetComponent<PlayerController>().itemUI = GameObject.Find("Player1_UI").GetComponent<UIController>();
        p2.GetComponent<PlayerController>().itemUI = GameObject.Find("Player2_UI").GetComponent<UIController>();
    }
}