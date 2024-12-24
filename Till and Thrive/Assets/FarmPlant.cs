using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmPlant : MonoBehaviour
{
    public Tilemap farmingTilemap; // Tilemap tempat bertani
    public Transform player; // Transform player (untuk posisi)
    private MoneyComponent moneyComponent; // Referensi ke MoneyComponent

    public List<TileBase> plantTiles = new List<TileBase>();
    public List<TileBase> stage2Tiles = new List<TileBase>();
    public List<TileBase> stage3Tiles = new List<TileBase>();
    public List<TileBase> finalTiles = new List<TileBase>();
    public TileBase emptyTile; // Tile kosong setelah panen

    private int selectedPlantIndex = 0; // Indeks tanaman yang dipilih
    private Dictionary<Vector3Int, int> plantedTiles = new Dictionary<Vector3Int, int>(); // Melacak jenis tanaman

    void Start()
    {
        moneyComponent = player.GetComponent<MoneyComponent>();

        if (moneyComponent == null)
        {
            Debug.LogError("MoneyComponent not found on player object!");
        }
    }

    void Update()
    {
        HandlePlantSelection();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlantTileAtPlayerPosition();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            HarvestTileAtPlayerPosition();
        }
    }

    void HandlePlantSelection()
    {
        // Pemilihan tanaman dengan angka dan backspace
        if (Input.GetKeyDown(KeyCode.Alpha1) && plantTiles.Count > 0) selectedPlantIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && plantTiles.Count > 1) selectedPlantIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && plantTiles.Count > 2) selectedPlantIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4) && plantTiles.Count > 3) selectedPlantIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5) && plantTiles.Count > 4) selectedPlantIndex = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6) && plantTiles.Count > 5) selectedPlantIndex = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7) && plantTiles.Count > 6) selectedPlantIndex = 6;
        if (Input.GetKeyDown(KeyCode.Alpha8) && plantTiles.Count > 7) selectedPlantIndex = 7;
        if (Input.GetKeyDown(KeyCode.Alpha9) && plantTiles.Count > 8) selectedPlantIndex = 8;
        if (Input.GetKeyDown(KeyCode.Alpha0) && plantTiles.Count > 9) selectedPlantIndex = 9;
        if (Input.GetKeyDown(KeyCode.Minus) && plantTiles.Count > 10) selectedPlantIndex = 10;
        if (Input.GetKeyDown(KeyCode.Equals) && plantTiles.Count > 11) selectedPlantIndex = 11;
        if (Input.GetKeyDown(KeyCode.Backspace) && plantTiles.Count > 12) selectedPlantIndex = 12;
    }

    void PlantTileAtPlayerPosition()
    {
        Vector3Int tilePosition = farmingTilemap.WorldToCell(player.position);
        TileBase currentTile = farmingTilemap.GetTile(tilePosition);

        if ((currentTile == null || currentTile == emptyTile) && selectedPlantIndex < plantTiles.Count)
        {
            farmingTilemap.SetTile(tilePosition, plantTiles[selectedPlantIndex]);
            plantedTiles[tilePosition] = selectedPlantIndex; // Catat jenis tanaman
            Debug.Log($"Planted {selectedPlantIndex + 1} at {tilePosition}");
            StartCoroutine(TransitionTile(tilePosition));
        }
        else
        {
            Debug.Log("Cannot plant here, tile is not empty!");
        }
    }

    IEnumerator TransitionTile(Vector3Int tilePosition)
    {
        if (!plantedTiles.ContainsKey(tilePosition)) yield break; // Pastikan tile ada di daftar

        int plantIndex = plantedTiles[tilePosition];

        if (plantIndex >= stage2Tiles.Count || plantIndex >= stage3Tiles.Count || plantIndex >= finalTiles.Count)
        {
            Debug.LogError("Transition tiles are not properly set up!");
            yield break;
        }

        // Tunggu 5 detik, lalu ganti ke stage 2
        yield return new WaitForSeconds(5f);
        farmingTilemap.SetTile(tilePosition, stage2Tiles[plantIndex]);

        // Tunggu 5 detik, lalu ganti ke stage 3
        yield return new WaitForSeconds(5f);
        farmingTilemap.SetTile(tilePosition, stage3Tiles[plantIndex]);

        // Tunggu 5 detik, lalu ganti ke stage final
        yield return new WaitForSeconds(5f);
        farmingTilemap.SetTile(tilePosition, finalTiles[plantIndex]);
    }

    void HarvestTileAtPlayerPosition()
    {
        Vector3Int tilePosition = farmingTilemap.WorldToCell(player.position);
        TileBase currentTile = farmingTilemap.GetTile(tilePosition);

        // Cek apakah posisi ada di plantedTiles
        if (plantedTiles.TryGetValue(tilePosition, out int plantIndex))
        {
            // Pastikan currentTile adalah finalTile dari jenis tanaman yang benar
            if (plantIndex < finalTiles.Count && currentTile == finalTiles[plantIndex])
            {
                // Tambahkan uang ke MoneyComponent
                moneyComponent.AddMoney(10); // Nilai panen

                // Ubah tile menjadi kosong setelah panen
                farmingTilemap.SetTile(tilePosition, emptyTile);
                plantedTiles.Remove(tilePosition); // Hapus dari dictionary
                Debug.Log($"Harvested plant {plantIndex + 1} at {tilePosition}");
            }
            else
            {
                Debug.Log("Cannot harvest, tile is not ready or does not match!");
            }
        }
        else
        {
            Debug.LogWarning($"No plant found at {tilePosition} to harvest!");
        }
    }

}
