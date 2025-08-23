using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public Transform mapSpawnPoint;
    private GameObject currentMap;

    public void LoadMap(MapData mapData)
    {
        if (currentMap != null)
            Destroy(currentMap);

        currentMap = Instantiate(mapData.mapPrefab, mapSpawnPoint.position, Quaternion.identity);
    }
}
