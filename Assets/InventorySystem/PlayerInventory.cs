using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public event Action OnInventoryChanged; // Event to notify when inventory changes

    private int maxSize = 30;
    public List<InventoryItem> inventory = new List<InventoryItem>();

    private ItemDatabase itemDatabase;

    void Start()
    {
        itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
        if (itemDatabase == null)
        {
            Debug.LogError("ItemDatabase not found in Resources folder.");
        }
        else
        {
            Debug.Log("ItemDatabase loaded successfully.");
        }
    }

    public void AddItem(string itemName, int quantity)
    {
        InventoryItem existingItem = inventory.Find(i => i.itemName == itemName);
        if (existingItem != null)
        {
            existingItem.quantity += quantity; // Increase quantity if already exists
            NotifyInventoryChanged(); // Notify UI of change
            Debug.Log("Item quantity updated: " + itemName);
            return;
        }

        if (inventory.Count >= maxSize)
        {
            // Inventory full handling
            Debug.LogWarning("Inventory is full.");
            return;
        }

        // Create and add new item
        ItemData itemData = itemDatabase.GetItemByName(itemName);
        if (itemData != null)
        {
            InventoryItem itemCopy = new InventoryItem(itemData.itemName, itemData.icon, itemData.description, quantity, itemData.price);
            inventory.Add(itemCopy);
            NotifyInventoryChanged(); // Notify UI of change
            Debug.Log("New item added: " + itemName);
        }
        else
        {
            Debug.LogWarning("Item not found in ItemDatabase: " + itemName);
        }
    }

    public void RemoveItem(string itemName, int quantity)
    {
        InventoryItem item = inventory.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.quantity -= quantity;
            if (item.quantity <= 0)
            {
                inventory.Remove(item);
                Debug.Log("Item removed: " + itemName);
            }
            else
            {
                Debug.Log("Item quantity reduced: " + itemName + ", new quantity: " + item.quantity);
            }
            NotifyInventoryChanged(); // Notify UI of change
        }
        else
        {
            Debug.LogWarning("Item not found in inventory: " + itemName);
        }
    }

    private void NotifyInventoryChanged()
    {
        OnInventoryChanged?.Invoke(); // Invoke the event if there are subscribers
    }
}
