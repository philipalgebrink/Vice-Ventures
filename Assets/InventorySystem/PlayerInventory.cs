using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int maxSize = 30;
    private int currentSize;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public void AddItem(InventoryItem item)
    {
        InventoryItem existingItem = inventory.Find(i => i.itemName == item.itemName);
        if (existingItem != null)
        {
            existingItem.quantity += item.quantity; // Increase quantity if already exists
            return;
        }

        if (currentSize >= maxSize)
        {
            // Don't add item
            // Inventory full
            return;
        }

        // Add item
        inventory.Add(item);

    }

    public void RemoveItem(InventoryItem item)
    {
        inventory.Remove(item);
        // Optionally: Update UI or trigger events based on inventory changes
    }
}
