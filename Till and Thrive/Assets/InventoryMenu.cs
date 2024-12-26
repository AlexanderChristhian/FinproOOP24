using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text[] slotTexts;        // Untuk menampilkan item di setiap slot
    public GameObject[] slotImages;     // Untuk menampilkan gambar item di setiap slot
    public int maxSlots = 6;            // Jumlah maksimum slot inventory
    public int maxItemPerSlot = 64;     // Jumlah item maksimal dalam satu slot

    [Header("Item Images")]
    public Sprite[] itemSprites;        // Array untuk menyimpan gambar item (FarmingPlant0, FarmingPlant1, ...)
    public Sprite emptySlotSprite;      // Gambar untuk slot kosong

    private List<InventorySlot> inventory; // Daftar slot inventory

    [System.Serializable]
    public class InventorySlot
    {
        public string itemName;
        public int quantity;
        public Sprite itemImage; // Gambar item (misal farming plant)
    }

    void Start()
    {
        // Inisialisasi inventory dengan slot kosong
        inventory = new List<InventorySlot>();
        for (int i = 0; i < maxSlots; i++)
        {
            inventory.Add(new InventorySlot() { itemName = "", quantity = 0 });
        }
        AddItem("FarmingPlant0", 10);
        // Pastikan UI diperbarui setelah inisialisasi
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (inventory[i].itemName != "")
            {
                // Set item name dan quantity di slot
                slotTexts[i].text = $"{inventory[i].quantity}";

                // Set gambar item di slot sesuai dengan itemName
                slotImages[i].GetComponent<UnityEngine.UI.Image>().sprite = GetItemSprite(inventory[i].itemName);
            }
            else
            {
                // Jika slot kosong, tampilkan teks dan gambar kosong
                slotTexts[i].text = "0";
                slotImages[i].GetComponent<UnityEngine.UI.Image>().sprite = emptySlotSprite;
            }
        }
    }

    // Fungsi untuk menambah item ke inventory
    public bool AddItem(string itemName, int quantity)
    {
        // Tentukan sprite item berdasarkan nama
        Sprite itemSprite = GetItemSprite(itemName);

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == itemName && inventory[i].quantity < maxItemPerSlot)
            {
                int spaceLeft = maxItemPerSlot - inventory[i].quantity;
                if (quantity <= spaceLeft)
                {
                    inventory[i].quantity += quantity;
                    inventory[i].itemImage = itemSprite; // Set image untuk item yang baru ditambahkan
                    UpdateInventoryUI();
                    return true;
                }
                else
                {
                    inventory[i].quantity += spaceLeft;
                    quantity -= spaceLeft;
                    inventory[i].itemImage = itemSprite; // Set image
                }
            }
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == "")
            {
                inventory[i].itemName = itemName;
                inventory[i].quantity = quantity;
                inventory[i].itemImage = itemSprite; // Set image
                UpdateInventoryUI();
                return true;
            }
        }

        Debug.Log("Inventory full! Cannot add more items.");
        return false;
    }

    // Fungsi untuk mendapatkan sprite berdasarkan item name
    private Sprite GetItemSprite(string itemName)
    {
        switch (itemName)
        {
            case "FarmingPlant0": return itemSprites[0];
            case "FarmingPlant1": return itemSprites[1];
            case "FarmingPlant2": return itemSprites[2];
            case "FarmingPlant3": return itemSprites[3];
            case "FarmingPlant4": return itemSprites[4];
            case "FarmingPlant5": return itemSprites[5];
            case "FarmingPlant6": return itemSprites[6];
            case "FarmingPlant7": return itemSprites[7];
            case "FarmingPlant8": return itemSprites[8];
            case "FarmingPlant9": return itemSprites[9];
            case "FarmingPlant10": return itemSprites[10];
            case "FarmingPlant11": return itemSprites[11];
            case "FarmingPlant12": return itemSprites[12];
            default: return emptySlotSprite; // Mengembalikan gambar slot kosong jika item tidak ditemukan
        }
    }

    // Fungsi untuk menghapus item dari inventory
    public void RemoveItem(string itemName, int quantity)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == itemName)
            {
                if (inventory[i].quantity >= quantity)
                {
                    inventory[i].quantity -= quantity;
                    if (inventory[i].quantity == 0)
                    {
                        inventory[i].itemName = "";
                        inventory[i].itemImage = emptySlotSprite; // Set image kosong jika item habis
                    }
                    UpdateInventoryUI();
                    return;
                }
                else
                {
                    quantity -= inventory[i].quantity;
                    inventory[i].itemName = "";
                    inventory[i].itemImage = emptySlotSprite; // Set image kosong
                    inventory[i].quantity = 0;
                }
            }
        }

        Debug.Log("Item not found or insufficient quantity.");
    }

    // Fungsi untuk mengupdate item pada slot tertentu
    public void UpdateSlot(int slotIndex, string itemName, int quantity)
    {
        if (slotIndex >= 0 && slotIndex < maxSlots)
        {
            inventory[slotIndex].itemName = itemName;
            inventory[slotIndex].quantity = quantity;
            inventory[slotIndex].itemImage = GetItemSprite(itemName); // Set image berdasarkan item
            UpdateInventoryUI();
        }
    }

    // Fungsi untuk memilih slot tertentu dan menampilkan isinya
    public void SelectSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < maxSlots)
        {
            InventorySlot slot = inventory[slotIndex];
            Debug.Log($"Slot {slotIndex + 1}: {slot.itemName} x {slot.quantity}");
        }
    }

    // Fungsi untuk mencetak seluruh inventory di log
    public void PrintInventory()
    {
        Debug.Log("=== Inventory ===");
        for (int i = 0; i < inventory.Count; i++)
        {
            Debug.Log($"Slot {i + 1}: {inventory[i].itemName} x {inventory[i].quantity}");
        }
    }

    public string GetSelectedItemName(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < maxSlots)
        {
            return inventory[slotIndex].itemName;
        }
        return "";
    }
}
