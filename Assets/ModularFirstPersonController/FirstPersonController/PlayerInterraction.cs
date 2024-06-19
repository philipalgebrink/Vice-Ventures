using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerInventory playerInventory;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Y))
        {
            playerInventory.AddItem(new InventoryItem("Test3", "Testest3", 5, 20));
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            playerInventory.AddItem(new InventoryItem("Test4", "Testest4", 5, 20));
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            playerInventory.AddItem(new InventoryItem("Test5", "Testest5", 5, 20));
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            playerInventory.AddItem(new InventoryItem("Test6", "Testest6", 5, 20));
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerInventory.AddItem(new InventoryItem("Test7", "Testest7", 5, 20));
        }
    }

    public void PickupItem(InventoryItem item)
    {
        playerInventory.AddItem(item);
        // Optionally: Play pickup sound, update UI, etc.
    }
}
