using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Databases/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;

    public ItemData GetItemByName(string itemName)
    {
        return items.Find(item => item.itemName == itemName);
    }

    public Sprite GetItemSprite(string itemName)
    {
        ItemData itemData = GetItemByName(itemName);
        if (itemData != null)
        {
            return itemData.icon;
        }
        return null;
    }
}
