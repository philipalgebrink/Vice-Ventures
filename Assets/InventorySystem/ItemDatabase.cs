using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public ItemData[] items;

    public ItemData GetItemByName(string itemName)
    {
        foreach (ItemData item in items)
        {
            if (item.itemName == itemName)
            {
                return item;
            }
        }
        return null;
    }
}
