using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmPlant : MonoBehaviour
{
    public Tilemap farmingTilemap; // Tilemap tempat bertani
    public TileBase plantTile; // Tile awal (Farming Plants_7)
    public TileBase stage2Tile; // Tile Farming Plants_8
    public TileBase stage3Tile; // Tile Farming Plants_9
    public TileBase finalTile; // Tile Farming Plants_10 (Final stage)
    public TileBase emptyTile; // Tile kosong setelah panen
    public Transform player; // Transform player (untuk posisi)
    private MoneyComponent moneyComponent; // Referensi ke MoneyComponent

    void Start()
    {
        // Ambil referensi komponen MoneyComponent dari GameObject Player
        moneyComponent = player.GetComponent<MoneyComponent>();

        if (moneyComponent == null)
        {
            Debug.LogError("MoneyComponent not found on player object!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlantTileAtPlayerPosition();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            HarvestTileAtPlayerPosition();
        }
    }

    void PlantTileAtPlayerPosition()
    {
        Vector3Int tilePosition = farmingTilemap.WorldToCell(player.position);
        TileBase currentTile = farmingTilemap.GetTile(tilePosition);

        if (currentTile == null || currentTile == emptyTile)
        {
            farmingTilemap.SetTile(tilePosition, plantTile);
            Debug.Log($"Planted at {tilePosition}");
            StartCoroutine(TransitionTile(tilePosition));
        }
        else
        {
            Debug.Log("Cannot plant here, tile is not empty!");
        }
    }

    IEnumerator TransitionTile(Vector3Int tilePosition)
    {
        // Tunggu 5 detik, lalu ganti ke Farming Plants_8
        yield return new WaitForSeconds(5f);
        farmingTilemap.SetTile(tilePosition, stage2Tile);

        // Tunggu 5 detik, lalu ganti ke Farming Plants_9
        yield return new WaitForSeconds(5f);
        farmingTilemap.SetTile(tilePosition, stage3Tile);

        // Tunggu 5 detik, lalu ganti ke Farming Plants_10
        yield return new WaitForSeconds(5f);
        farmingTilemap.SetTile(tilePosition, finalTile);
    }

    void HarvestTileAtPlayerPosition()
    {
        Vector3Int tilePosition = farmingTilemap.WorldToCell(player.position);
        TileBase currentTile = farmingTilemap.GetTile(tilePosition);

        if (currentTile == finalTile)
        {
            // Tambahkan uang ke MoneyComponent
            moneyComponent.AddMoney(10); // Nilai panen

            // Ubah tile menjadi kosong setelah panen
            farmingTilemap.SetTile(tilePosition, emptyTile);
            Debug.Log($"Harvested at {tilePosition}");
        }
        else
        {
            Debug.Log("Cannot harvest, tile is not ready!");
        }
    }
}
