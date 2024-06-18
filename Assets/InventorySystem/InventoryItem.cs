using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;
    public string description;
    public int quantity;
    public int price;

    public InventoryItem(string _itemName, string _description, int _quantity, int _price)
    {
        itemName = _itemName;
        description = _description;
        quantity = _quantity;
        price = _price;
    }
}