using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public InventoryManager inventoryManager; // Reference to the InventoryManager script
    public PlayerInventory playerInventory;
    private bool isEPressed = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isEPressed)
        {
            isEPressed = true;
            inventoryManager.UseItemInHand();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isEPressed = false;
        }
    }

    void PlantSeedInPot()
    {
        // Example: Perform action for planting seed in pot
        Debug.Log("[Vice] Planting Cannabis Seed in pot...");

        // Example: Remove the seed from hand (handled in UseItemInHand of InventoryManager)
        inventoryManager.UseItemInHand();
    }

    public void PlacePotOnGround()
    {
        InventoryItem itemInHand = inventoryManager.CurrentItemInHand;

        if (itemInHand != null && itemInHand.itemName == "Pot")
        {
            // Load the Pot prefab from Resources/Item folder
            GameObject potPrefab = Resources.Load<GameObject>("Items/Pot");

            if (potPrefab != null)
            {
                // Example: Instantiate the Pot prefab at the player's position
                Instantiate(potPrefab, transform.position, Quaternion.identity);
                Debug.Log("[Vice] Pot placed on ground.");

                // Remove one "Pot" from inventory
                playerInventory.RemoveItem(itemInHand.itemName, 1);

                // Clear item from hand
                inventoryManager.CurrentItemInHand = null;
                Debug.Log("[Vice] Quantity should have decreased");
                Debug.Log("[Vice] Should not have anything in hand");
            }
            else
            {
                Debug.Log("[Vice] Pot prefab not found in Resources/Item folder.");
            }
        }
        else if (itemInHand == null)
        {
            Debug.Log("[Vice] No Pot in hand to place on ground.");
        }
        else
        {
            Debug.Log("[Vice] Item in hand is not a Pot.");
        }
    }
}
