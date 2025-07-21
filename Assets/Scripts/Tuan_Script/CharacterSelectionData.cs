using UnityEngine;

[System.Serializable]
public class SelectedCharacterData
{
    public string characterName;
    public Sprite characterSprite;
    public Sprite[] skillSprites;
    public GameObject characterPrefab;
}

public class CharacterSelectionData : MonoBehaviour
{
    public static CharacterSelectionData Instance;

    public SelectedCharacterData player1Character;
    public SelectedCharacterData player2Character;

    public GameObject player1Instance;
    public GameObject player2Instance;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại qua scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayer1(SelectedCharacterData data)
    {
        player1Character = data;
    }

    public void SetPlayer2(SelectedCharacterData data)
    {
        player2Character = data;
    }

    public void RemoveDeadPlayer(PlayerController deadPlayer)
    {
        if (player1Instance == deadPlayer.gameObject)
        {
            Destroy(player1Instance);
            player1Instance = null;
        }
        else if (player2Instance == deadPlayer.gameObject)
        {
            Destroy(player2Instance);
            player2Instance = null;
        }
    }
}
