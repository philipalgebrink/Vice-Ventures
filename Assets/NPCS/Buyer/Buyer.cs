using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buyer : NpcBase
{
    public override void Interact(PlayerInteraction playerInteraction)
    {
        Debug.Log("[Vice] Buyer: Interact method called.");
        // GameManager.instance.AddItemToHand(new InventoryItem("Cannabis", null, "A plant that can be harvested for its buds.", 1, 10, null));
    }

    // Buyer Interactions
    public void HandleBuyerInteraction()
    {
        Debug.Log("[Vice] Interacting with: Buyer");
        // BuyerUIManager.Instance.OpenShop(this);
    }

}
