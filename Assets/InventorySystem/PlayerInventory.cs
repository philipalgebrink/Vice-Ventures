using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public event Action OnInventoryChanged; // Event to notify when inventory changes
    public ItemDatabase itemDatabase; // Reference to the ItemDatabase scriptable object

    private int maxSize = 30;
    public List<InventoryItem> inventory = new List<InventoryItem>();

    public void AddItem(string itemName, int quantity)
    {
        ItemData itemData = itemDatabase.GetItemByName(itemName);
        if (itemData == null)
        {
            Debug.LogWarning("Item not found in database: " + itemName);
            return;
        }

        InventoryItem existingItem = inventory.Find(i => i.itemName == itemName);
        if (existingItem != null)
        {
            existingItem.quantity += quantity; // Increase quantity if already exists
            NotifyInventoryChanged(); // Notify UI of change
            return;
        }

        if (inventory.Count >= maxSize)
        {
            Debug.LogWarning("Inventory is full!");
            return;
        }

        // Add new item
        inventory.Add(new InventoryItem(itemData, quantity));
        NotifyInventoryChanged(); // Notify UI of change
    }

    public void RemoveItem(InventoryItem item)
    {
        inventory.Remove(item);
        NotifyInventoryChanged(); // Notify UI of change
    }

    private void NotifyInventoryChanged()
    {
        OnInventoryChanged?.Invoke(); // Invoke the event if there are subscribers
    }
}
