using UnityEngine;

[CreateAssetMenu(fileName = "NewMapData", menuName = "Game/Map Data")]
public class MapData : ScriptableObject
{
    public string mapName;
    public MapType type;
    public GameObject mapPrefab;
    public Sprite previewImage;
}