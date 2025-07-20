using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public string characterName;
        public Sprite characterSprite;
        public Sprite[] skillSprites;
    }

    public CharacterData[] characters;
    public Image displayCharacterImage;
    public Image[] skillImages;

    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= characters.Length) return;

        // Hiển thị hình ảnh nhân vật
        displayCharacterImage.sprite = characters[index].characterSprite;

        // Hiển thị skill tương ứng
        for (int i = 0; i < skillImages.Length; i++)
        {
            if (i < characters[index].skillSprites.Length)
            {
                skillImages[i].sprite = characters[index].skillSprites[i];
                skillImages[i].gameObject.SetActive(true);
            }
            else
            {
                skillImages[i].gameObject.SetActive(false);
            }
        }

        SelectedCharacterData selected = new SelectedCharacterData
        {
            characterName = characters[index].characterName,
            characterSprite = characters[index].characterSprite,
            skillSprites = characters[index].skillSprites
        };

        if (gameObject.name.Contains("Player1"))
        {
            CharacterSelectionData.Instance.SetPlayer1(selected);
        }
        else if (gameObject.name.Contains("Player2"))
        {
            CharacterSelectionData.Instance.SetPlayer2(selected);
        }
    }
}
