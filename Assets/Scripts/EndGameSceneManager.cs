using UnityEngine;
using UnityEngine.UI;

public class EndGameSceneManager : MonoBehaviour
{
    public Image winnerPlayerImage; // Kéo UI Image "WinnerPlayer" vào đây

    void Start()
    {
        var data = CharacterSelectionData.Instance;
        Sprite winnerSprite = null;

        // ✅ Cách 1: Nếu đã lưu sẵn Sprite khi chọn nhân vật (CharacterSelectionData)
        if (data.player1Instance != null)
        {
            winnerSprite = data.player1Character.characterSprite;
        }
        else if (data.player2Instance != null)
        {
            winnerSprite = data.player2Character.characterSprite;
        }

        // ✅ Cách 2 (dự phòng): Nếu chưa lưu sẵn, lấy trực tiếp từ SpriteRenderer của Player còn sống
        if (winnerSprite == null)
        {
            if (data.player1Instance != null)
                winnerSprite = data.player1Instance.GetComponent<SpriteRenderer>().sprite;
            else if (data.player2Instance != null)
                winnerSprite = data.player2Instance.GetComponent<SpriteRenderer>().sprite;
        }

        // ✅ Đổi Sprite của UI Image
        if (winnerSprite != null)
        {
            winnerPlayerImage.sprite = winnerSprite;
            winnerPlayerImage.preserveAspect = true; // giữ đúng tỉ lệ ảnh
        }
        else
        {
            Debug.LogWarning("Không tìm thấy người thắng hoặc sprite!");
        }

        // ✅ Hủy player còn sống để không rơi trong scene
        if (data.player1Instance != null)
        {
            Destroy(data.player1Instance);
            data.player1Instance = null;
        }
        if (data.player2Instance != null)
        {
            Destroy(data.player2Instance);
            data.player2Instance = null;
        }
    }
}
