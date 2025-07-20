using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BasicTile", menuName = "Custom Tile/BasicTile")]
public class BasicTile : Tile
{
    public string tileID; // Dùng để nhận diện loại platform
}