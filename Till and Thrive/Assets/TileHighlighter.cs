using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHighlighter : MonoBehaviour
{
    public Tilemap farmingTilemap; // Tilemap tempat menanam
    public Tilemap highlightTilemap; // Tilemap untuk highlight
    public Transform player; // Player
    public TileBase highlightTile; // Tile khusus untuk highlight
    private bool isHighlighting = false; // Status toggle highlight

    void Update()
    {
        // Toggle highlight dengan tombol H
        if (Input.GetKeyDown(KeyCode.H))
        {
            isHighlighting = !isHighlighting;
            if (!isHighlighting)
            {
                // Bersihkan highlight saat dimatikan
                highlightTilemap.ClearAllTiles();
            }
        }

        if (isHighlighting)
        {
            HighlightPlayerTile();
        }
    }

    void HighlightPlayerTile()
    {
        // Hitung posisi tile berdasarkan posisi Player
        Vector3Int tilePosition = farmingTilemap.WorldToCell(player.position);

        // Bersihkan highlight sebelumnya
        highlightTilemap.ClearAllTiles();

        // Set tile di posisi player untuk highlight
        highlightTilemap.SetTile(tilePosition, highlightTile);
    }
}
