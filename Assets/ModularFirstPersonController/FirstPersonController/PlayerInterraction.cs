using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerInventory playerInventory;

    public void PickupItem(InventoryItem item)
    {
        playerInventory.AddItem(item);
        // Optionally: Play pickup sound, update UI, etc.
    }
}
