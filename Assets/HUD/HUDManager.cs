using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public InventoryManager inventoryManager; // Reference to InventoryManager
    public Image itemInHandImage; // Reference to the Image component displaying item in hand

    void Start()
    {
        UpdateItemInHandUI(); // Initialize UI when scene starts
    }

    void Update()
    {
        // Optionally, update UI continuously if item in hand can change dynamically
        UpdateItemInHandUI();
    }

    public void UpdateItemInHandUI() // Change accessibility to public
    {
        if (itemInHandImage != null && inventoryManager != null)
        {
            // Get the sprite of the item in hand from InventoryManager
            Sprite itemSprite = inventoryManager.GetItemInHandSprite();

            // Update the UI image with the sprite of the item in hand
            itemInHandImage.sprite = itemSprite;

            // Set alpha based on whether item is in hand
            Color imageColor = itemInHandImage.color;
            imageColor.a = itemSprite != null ? 1f : 0f; // Set alpha to 1 if itemSprite is not null, otherwise 0
            itemInHandImage.color = imageColor;
        }
    }
}
