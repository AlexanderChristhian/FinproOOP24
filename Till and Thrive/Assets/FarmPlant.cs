using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmPlant : MonoBehaviour
{
    public Tilemap farmingTilemap; // Tilemap tempat bertani
    public Transform player; // Transform player (untuk posisi)

    public TileBase farmLandTile; // Tile sawah yang akan diberikan
    public Tilemap farmground; // Tilemap tempat bertani layer 1
    private bool farmLandAdded = false; 
    private MoneyComponent moneyComponent; // Referensi ke MoneyComponent
    public InventoryMenu inventoryMenu; // Referensi ke InventoryMenu untuk mendapatkan item yang dipilih

    private string selectedItemName; // Item yang dipilih dari inventory
    public List<TileBase> plantTiles = new List<TileBase>();
    public List<TileBase> stage2Tiles = new List<TileBase>();
    public List<TileBase> stage3Tiles = new List<TileBase>();
    public List<TileBase> finalTiles = new List<TileBase>();
    public TileBase emptyTile; // Tile kosong setelah panen

    private int selectedPlantIndex = -1; // Indeks tanaman yang dipilih berdasarkan inventory
    private Dictionary<Vector3Int, int> plantedTiles = new Dictionary<Vector3Int, int>(); // Melacak jenis tanaman

    private float wateringInterval = 3f; // Interval waktu untuk penyiraman tanaman
    private float lastWateringTime = 0f; // Waktu terakhir tanaman disiram
    private bool needsWatering = false; // Apakah tanaman membutuhkan penyiraman

    void Start()
    {
        moneyComponent = player.GetComponent<MoneyComponent>();

        if (moneyComponent == null)
        {
            Debug.LogError("MoneyComponent not found on player object!");
        }

        // Ambil referensi ke InventoryMenu
        if (inventoryMenu == null)
        {
            Debug.LogError("InventoryMenu not assigned!");
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

        if (Input.GetKeyDown(KeyCode.G))
        {
            AddFarmLandTileAtPlayerPosition();
        }

        // Cek apakah tanaman membutuhkan penyiraman setiap interval waktu
        if (Time.time - lastWateringTime >= wateringInterval)
        {
            CheckWateringRequirement();
        }

        // Penyiraman oleh player jika berada di atas tile tanaman dan menggunakan item penyiraman
        if (needsWatering && Input.GetKeyDown(KeyCode.E))
        {
            WaterPlantAtPlayerPosition();
        }
    }

    void AddFarmLandTileAtPlayerPosition()
    {
        if (farmLandTile == null)
        {
            Debug.LogError("FarmLandTile is not assigned!");
            return;
        }

        Vector3Int tilePosition = farmground.WorldToCell(player.position);
        TileBase currentTile = farmground.GetTile(tilePosition);

        // Hanya tambahkan tile sawah jika posisi saat ini kosong
        if (currentTile == null || currentTile == emptyTile)
        {
            farmground.SetTile(tilePosition, farmLandTile);
            farmLandAdded = true;
            Debug.Log("Farm Land tile added at position: " + tilePosition);
        }
        else
        {
            Debug.Log("Tile is already occupied.");
        }
    }
    void HandlePlantSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            selectedPlantIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            selectedPlantIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            selectedPlantIndex = 2;
        }

        if (selectedPlantIndex == -1) return; // Pastikan ada item yang dipilih
        
        // Get the selected item name from inventory and update the plant index
        selectedItemName = inventoryMenu.GetSelectedItemName(selectedPlantIndex);
        selectedPlantIndex = GetPlantIndexFromItemName(selectedItemName);
    }

    void CheckWateringRequirement()
    {
        Vector3Int tilePosition = farmingTilemap.WorldToCell(player.position);
        TileBase currentTile = farmingTilemap.GetTile(tilePosition);

        // Cek apakah tile adalah tanaman yang bisa disiram
        if (plantedTiles.ContainsKey(tilePosition) && currentTile != null && currentTile != emptyTile)
        {
            needsWatering = true;
        }
        else
        {
            needsWatering = false;
        }

        lastWateringTime = Time.time; // Update waktu terakhir penyiraman
    }

    public void WaterPlantAtPlayerPosition()
    {
        Vector3Int tilePosition = farmingTilemap.WorldToCell(player.position);
        TileBase currentTile = farmingTilemap.GetTile(tilePosition);

        // Periksa apakah tanaman ada di posisi tersebut dan memerlukan penyiraman
        if (plantedTiles.ContainsKey(tilePosition) && currentTile != null && currentTile != emptyTile)
        {
            Debug.Log("Plant watered!");
            needsWatering = false; // Tanaman sudah disiram
        }
        else
        {
            Debug.Log("No plant to water at this position.");
        }
    }

    int GetPlantIndexFromItemName(string itemName)
    {
        switch (itemName)
        {
            case "FarmingPlant0": return 0;
            case "FarmingPlant1": return 1;
            case "FarmingPlant2": return 2;
            case "FarmingPlant3": return 3;
            case "FarmingPlant4": return 4;
            case "FarmingPlant5": return 5;
            case "FarmingPlant6": return 6;
            case "FarmingPlant7": return 7;
            case "FarmingPlant8": return 8;
            case "FarmingPlant9": return 9;
            case "FarmingPlant10": return 10;
            case "FarmingPlant11": return 11;
            case "FarmingPlant12": return 12;
            default: return -1;
        }
    }

    void PlantTileAtPlayerPosition()
    {
        if (selectedPlantIndex == -1) return;

        Vector3Int tilePosition = farmingTilemap.WorldToCell(player.position);
        TileBase currentTile = farmingTilemap.GetTile(tilePosition);
        Vector3Int farmGroundTilePosition = farmground.WorldToCell(player.position);
        TileBase farmGroundTile = farmground.GetTile(farmGroundTilePosition);

        if (farmGroundTile != farmLandTile)
        {
            Debug.Log("Cannot plant! The ground is not prepared.");
            return; // Hentikan proses jika tanah bukan farmland
        }
        if ((currentTile == null || currentTile == emptyTile) && selectedPlantIndex < plantTiles.Count)
        {
            farmingTilemap.SetTile(tilePosition, plantTiles[selectedPlantIndex]);
            plantedTiles[tilePosition] = selectedPlantIndex; // Catat jenis tanaman
            StartCoroutine(TransitionTile(tilePosition));

            inventoryMenu.RemoveItem(selectedItemName, 1);
        }
    }

    IEnumerator TransitionTile(Vector3Int tilePosition)
    {
        
        yield return new WaitForSeconds(5f);
        farmingTilemap.SetTile(tilePosition, stage2Tiles[plantedTiles[tilePosition]]);
        yield return new WaitForSeconds(5f);
        farmingTilemap.SetTile(tilePosition, stage3Tiles[plantedTiles[tilePosition]]);
        yield return new WaitForSeconds(5f);
        farmingTilemap.SetTile(tilePosition, finalTiles[plantedTiles[tilePosition]]);
    }

    void HarvestTileAtPlayerPosition()
    {
        Vector3Int tilePosition = farmingTilemap.WorldToCell(player.position);
        TileBase currentTile = farmingTilemap.GetTile(tilePosition);

        if (plantedTiles.TryGetValue(tilePosition, out int plantIndex))
        {
            if (currentTile == finalTiles[plantIndex])
            {
                moneyComponent.AddMoney(10); // Menambahkan uang
                farmingTilemap.SetTile(tilePosition, emptyTile);
                plantedTiles.Remove(tilePosition);
            }
        }
    }
}
