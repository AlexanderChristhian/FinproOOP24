using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button[] buttons; // Array dari 16 tombol
    public GameObject menuPanel; // Panel yang berisi menu yang akan ditampilkan/disembunyikan
    
    [Header("Game Settings")]
    public MoneyComponent moneyComponent; // Referensi ke MoneyComponent
    public int[] itemPrices;      // Harga untuk masing-masing item
    public InventoryMenu inventoryManager; // Referensi ke InventoryManager

    private string[] itemNames = {
        "FarmingPlant0", "FarmingPlant1", "FarmingPlant2", 
        "FarmingPlant3", "FarmingPlant4", "FarmingPlant5",
        "FarmingPlant6", "FarmingPlant7", "FarmingPlant8",
        "FarmingPlant9", "FarmingPlant10", "FarmingPlant11", 
        "FarmingPlant12"
    };

    private bool isMenuActive = true; // Menyimpan status apakah menu aktif atau tidak

    void Start()
    {
        // Pastikan jumlah tombol sesuai dengan jumlah item
        if (buttons.Length != itemNames.Length || buttons.Length != itemPrices.Length)
        {
            Debug.LogError("Jumlah tombol, nama item, atau harga item tidak cocok!");
            return;
        }

        // Assign listener untuk setiap tombol
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Perlu untuk menghindari capture closure issue
            buttons[i].onClick.AddListener(() => BuyItem(index));
        }

        UpdateButtonUI();
        UpdateMenuVisibility(); // Memastikan menu tampil pada saat mulai
    }

    void Update()
    {
        UpdateButtonUI();
    }

    // Fungsi untuk membeli item
    public void BuyItem(int index)
    {
        string itemName = itemNames[index];
        int itemPrice = itemPrices[index];

        // Cek apakah pemain memiliki cukup uang menggunakan MoneyComponent
        if (moneyComponent.money >= itemPrice)
        {
            moneyComponent.SubtractMoney(itemPrice); // Kurangi uang dari MoneyComponent
            Debug.Log($"Berhasil membeli {itemName} seharga {itemPrice} koin. Sisa uang: {moneyComponent.money}");

            // Tambahkan item ke inventory setelah berhasil membeli
            inventoryManager.AddItem(itemName, 1); // Menambahkan satu item ke inventory

            UpdateButtonUI();
        }
        else
        {
            Debug.Log($"Uang tidak cukup untuk membeli {itemName}! Harga: {itemPrice}, Uang: {moneyComponent.money}");
        }

        UpdateButtonUI();
    }

    // Update tampilan tombol berdasarkan uang pemain
    void UpdateButtonUI()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            Text buttonText = button.GetComponentInChildren<Text>();
            
            if (moneyComponent.money >= itemPrices[i])
            {
                button.interactable = true; // Aktifkan tombol jika cukup uang
            }
            else
            {
                button.interactable = false; // Nonaktifkan tombol jika uang tidak cukup
            }

            // Perbarui teks tombol
            if (buttonText != null)
            {
                buttonText.text = $"{itemNames[i]} (${itemPrices[i]})";
            }
        }
    }

    // Fungsi untuk toggle visibilitas menu
    void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        UpdateMenuVisibility(); // Update tampilan menu sesuai dengan status
    }

    // Fungsi untuk memperbarui visibilitas menu
    void UpdateMenuVisibility()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(isMenuActive); // Mengaktifkan/menonaktifkan menu
        }
    }
}
