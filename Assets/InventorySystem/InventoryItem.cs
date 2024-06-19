using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;
    public string description;
    public int quantity;
    public int price;
    public Sprite sprite;

    public InventoryItem(string _itemName, Sprite _icon, string _description, int _quantity, int _price, Sprite _sprite)
    {
        itemName = _itemName;
        icon = _icon;
        description = _description;
        quantity = _quantity;
        price = _price;
        sprite = _sprite;
    }
}
