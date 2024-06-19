using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItem : MonoBehaviour
{
    public InventoryItem item; // Reference to the InventoryItem associated with this UI element

    public void OnItemClick()
    {
        GameManager.instance.AddItemToHand(item); // Calls GameManager method to add item to hand
    }
}
