using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;

     //Kích hoạt item (dùng cho buff active hoặc vũ khí)
    public abstract void Activate(PlayerController player);
}
