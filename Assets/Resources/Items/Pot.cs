using UnityEngine;

public class Pot : Interactable
{
    public override void Interact(PlayerInteraction playerInteraction)
    {
        // Handle interaction with Pot in PlayerInteraction script.
        playerInteraction.HandlePotInteraction(this);
    }
}
