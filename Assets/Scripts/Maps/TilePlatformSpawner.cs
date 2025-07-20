using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilePlatformSpawner : MonoBehaviour
{
    public Tilemap tilemap;

    [System.Serializable]
    public class TileEntry
    {
        public string tileID;
        public GameObject prefab;
    }

    public List<TileEntry> tileMappings;

    void Start()
    {
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile is BasicTile basicTile)
            {
                foreach (var entry in tileMappings)
                {
                    if (entry.tileID == basicTile.tileID)
                    {
                        Vector3 worldPos = tilemap.CellToWorld(pos) + tilemap.tileAnchor;
                        Instantiate(entry.prefab, worldPos, Quaternion.identity);
                        Debug.Log($"Spawning tileID: {basicTile.tileID} at {pos}");
                        break;
                    }
                }
            }
        }

        tilemap.gameObject.SetActive(false); // Ẩn tile sau khi spawn xong
    }
}
