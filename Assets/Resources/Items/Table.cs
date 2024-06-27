using UnityEngine;
using System.Collections.Generic;

public class Table : Interactable
{
    private bool hasEmptyZipBag = false;
    private bool hasUnpackedCannabis = false;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        playerInteraction.HandleTableInteraction(this);
    }

    public bool AddItem(string itemName)
    {
        if (itemName == "Empty Zipbag" && !hasEmptyZipBag)
        {
            hasEmptyZipBag = true;
            return true;
        }
        else if (itemName == "Unpacked Cannabis" && !hasUnpackedCannabis)
        {
            hasUnpackedCannabis = true;
            return true;
        }
        return false;
    }

    public bool CanCombine()
    {
        return hasEmptyZipBag && hasUnpackedCannabis;
    }

    public void ResetTable()
    {
        hasEmptyZipBag = false;
        hasUnpackedCannabis = false;
    }
}