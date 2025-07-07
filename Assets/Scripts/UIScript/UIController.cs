using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class UIController : MonoBehaviour
{
    public List<Image> itemSlotImages;      // khung ngoài
    public List<Image> itemIcons;           // icon con nằm bên trong
    public Sprite emptyIcon;

    void Start()
    {
        for (int i = 0; i < itemIcons.Count; i++)
        {
            ClearItemSlot(i); // khởi tạo icon trống ban đầu
        }
    }

    public void SetItemIcon(int index, Sprite icon)
    {
        if (index < 0 || index >= itemIcons.Count) return;
        itemIcons[index].sprite = icon;
        itemIcons[index].color = Color.white;
    }

    public void ClearItemSlot(int index)
    {
        if (index < 0 || index >= itemIcons.Count) return;
        itemIcons[index].sprite = null;
        itemIcons[index].color = Color.clear; // ẩn icon
    }
}
