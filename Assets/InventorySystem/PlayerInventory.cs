using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public event Action OnInventoryChanged; // Event to notify when inventory changes

    private int maxSize = 30;
    private int currentSize;
    public List<InventoryItem> inventory = new List<InventoryItem>();

    public void AddItem(InventoryItem item)
    {
        InventoryItem existingItem = inventory.Find(i => i.itemName == item.itemName);
        if (existingItem != null)
        {
            existingItem.quantity += item.quantity; // Increase quantity if already exists
            NotifyInventoryChanged(); // Notify UI of change
            return;
        }

        if (currentSize >= maxSize)
        {
            // Inventory full handling
            return;
        }

        // Add item
        inventory.Add(item);
        NotifyInventoryChanged(); // Notify UI of change
    }

    public void RemoveItem(InventoryItem item)
    {
        inventory.Remove(item);
        NotifyInventoryChanged(); // Notify UI of change
        // Optionally: Update UI or trigger events based on inventory changes
    }

    private void NotifyInventoryChanged()
    {
        OnInventoryChanged?.Invoke(); // Invoke the event if there are subscribers
    }
}
