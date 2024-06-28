using System.Collections.Generic;
using UnityEngine;

public class Seller : Interactable
{
    public List<ItemData> itemsForSale = new List<ItemData>(); // Ensure this is of type ItemData

    public override void Interact(PlayerInteraction playerInteraction)
    {
        Debug.Log("[Vice] Interacting with: Seller");
        HandleSellerInteraction();
    }

    // Seller Interactions
    public void HandleSellerInteraction()
    {
        // Debug.Log("[Vice] Interacting with: Seller");
        ShopUIManager.Instance.OpenShop(this);
    }
}
