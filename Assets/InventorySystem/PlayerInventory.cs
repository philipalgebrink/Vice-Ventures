using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    private int maxSize = 30;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public event System.Action OnInventoryChanged;
    private InventoryItem currentItemInHand; // Track the item currently in hand
    public ItemDatabase itemDatabase;

    void Start()
    {
        itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
        if (itemDatabase == null)
        {
            Debug.Log("[Vice] ItemDatabase not found in Resources folder.");
        }
        else
        {
            Debug.Log("[Vice] ItemDatabase loaded successfully.");
        }
    }

    public void AddItem(string itemName, int quantity)
    {
        InventoryItem existingItem = inventory.Find(i => i.itemName == itemName);
        if (existingItem != null)
        {
            existingItem.quantity += quantity; // Increase quantity if already exists
            NotifyInventoryChanged();
            Debug.Log("[Vice] Item quantity updated: " + itemName);
            return;
        }

        if (inventory.Count >= maxSize)
        {
            // Inventory full handling
            Debug.Log("[Vice] Inventory is full.");
            return;
        }

        // Create and add new item
        ItemData itemData = itemDatabase.GetItemByName(itemName);
        if (itemData != null)
        {
            InventoryItem itemCopy = new InventoryItem(itemData.itemName, itemData.icon, itemData.description, quantity, itemData.price, itemData.sprite);
            inventory.Add(itemCopy);
            NotifyInventoryChanged();
            Debug.Log("[Vice] New item added: " + itemName);
        }
        else
        {
            Debug.Log("[Vice] Item not found in ItemDatabase: " + itemName);
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
                Debug.Log("[Vice] Item removed: " + itemName);
            }
            else
            {
                Debug.Log("[Vice] Item quantity reduced: " + itemName + ", new quantity: " + item.quantity);
            }
            NotifyInventoryChanged();
        }
        else
        {
            Debug.Log("[Vice] Item not found in inventory: " + itemName);
        }
    }


    public void NotifyInventoryChanged()
    {
        Debug.Log("[Vice] Inventory changed notification sent.");
        OnInventoryChanged?.Invoke();
    }
}