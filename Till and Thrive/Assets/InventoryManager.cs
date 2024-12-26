using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public class InventorySlot
    {
        public string itemName;
        public int quantity;

        public InventorySlot(string name, int count)
        {
            itemName = name;
            quantity = count;
        }
    }

    public int maxSlots = 6;
    public int maxItemPerSlot = 64;
    private List<InventorySlot> inventory;
    public int selectedSlot = 0; // Slot yang dipilih (0-based index)

    void Start()
    {
        if (inventory == null)
        {
            inventory = new List<InventorySlot>();
            Debug.Log("Inventory telah diinisialisasi!");
        }
        else
        {
            Debug.Log("Inventory sudah ada, tidak perlu inisialisasi ulang.");
        }

        // Isi inventory jika belum ada item (contoh pengujian)
        if (inventory.Count == 0)
        {
            inventory.Add(new InventorySlot("FarmingPlant0", 5));
            inventory.Add(new InventorySlot("FarmingPlant1", 3));
        }
    }

    public bool AddItem(string itemName, int quantity)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == itemName && inventory[i].quantity < maxItemPerSlot)
            {
                int spaceLeft = maxItemPerSlot - inventory[i].quantity;
                if (quantity <= spaceLeft)
                {
                    inventory[i].quantity += quantity;
                    return true;
                }
                else
                {
                    inventory[i].quantity += spaceLeft;
                    quantity -= spaceLeft;
                }
            }
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == "")
            {
                inventory[i].itemName = itemName;
                if (quantity <= maxItemPerSlot)
                {
                    inventory[i].quantity = quantity;
                    return true;
                }
                else
                {
                    inventory[i].quantity = maxItemPerSlot;
                    quantity -= maxItemPerSlot;
                }
            }
        }

        Debug.Log("Inventory full! Cannot add more items.");
        return false;
    }

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
                    }
                    return;
                }
                else
                {
                    quantity -= inventory[i].quantity;
                    inventory[i].itemName = "";
                    inventory[i].quantity = 0;
                }
            }
        }
    }

    public void SelectSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < maxSlots)
        {
            selectedSlot = slotIndex;
            Debug.Log($"Selected Slot {selectedSlot + 1}: {GetSlotInfo(selectedSlot)}");
        }
        else
        {
            Debug.Log("Invalid slot index!");
        }
    }

    public string GetSlotInfo(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventory.Count)
            return "Invalid Slot";

        InventorySlot slot = inventory[slotIndex];
        return slot.itemName != "" ? $"{slot.itemName} x {slot.quantity}" : "Empty";
    }

    public void PrintInventory()
    {
        Debug.Log("=== Inventory ===");
        for (int i = 0; i < inventory.Count; i++)
        {
            Debug.Log($"Slot {i + 1}: {GetSlotInfo(i)}");
        }
    }

    public List<InventorySlot> GetInventory()
    {
        return inventory;
    }

}
