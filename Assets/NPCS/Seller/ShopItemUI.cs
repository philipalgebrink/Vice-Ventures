using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public Button buyButton;
    private PlayerInventory playerInventory;
    private PlayerStats playerStats;

    public void Setup(ItemData itemData, PlayerInventory inventory, PlayerStats stats)
    {
        playerInventory = inventory;
        playerStats = stats;

        icon.sprite = itemData.icon;
        itemName.text = itemData.itemName;
        itemPrice.text = "$ " + itemData.price.ToString();

        // Ensure no duplicate listeners
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => BuyItem(itemData));
    }

    private void BuyItem(ItemData itemData)
    {
        if (playerStats.money >= itemData.price)
        {
            playerInventory.AddItem(itemData.itemName, 1);
            playerStats.money -= itemData.price;
            Debug.Log($"Bought {itemData.itemName} for {itemData.price} $.");
        }
        else
        {
            Debug.Log("Not enough $ to buy this item.");
        }
    }
}