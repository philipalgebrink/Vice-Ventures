using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerInventory playerInventory;
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.R))
        {
            playerInventory.AddItem(new InventoryItem("Test", "Testest", 2, 10));
        }
    }

    public void PickupItem(InventoryItem item)
    {
        playerInventory.AddItem(item);
        // Optionally: Play pickup sound, update UI, etc.
    }
}
