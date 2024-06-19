using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public InventoryItem item; // Item stored in this slot
    public InventoryManager inventoryManager; // Reference to the InventoryManager

    public void OnSlotClick()
    {
        Debug.Log("[Vice] Clicked on inventory slot with item: " + item.itemName); // Debug statement to verify click

        if (item != null && inventoryManager != null)
        {
            inventoryManager.PutItemInHand(item); // Call method in InventoryManager to put item in hand
        }
    }
}
