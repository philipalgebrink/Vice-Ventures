using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem item;
    public InventoryManager inventoryManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            inventoryManager.SpawnItem(item);
        }
    }

    public void SetItem(InventoryItem newItem)
    {
        item = newItem;
    }
}
