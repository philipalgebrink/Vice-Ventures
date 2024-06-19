using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;
    public string description;
    public int quantity;
    public int price;

    public InventoryItem(ItemData itemData, int _quantity)
    {
        itemName = itemData.itemName;
        icon = itemData.icon;
        description = itemData.description;
        quantity = _quantity;
        price = itemData.price;
    }
}
