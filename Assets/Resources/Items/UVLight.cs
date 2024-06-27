using UnityEngine;

public class UVLight : Interactable
{
    public override void Interact(PlayerInteraction playerInteraction)
    {
        // Handle interaction with UVLight in PlayerInteraction script.
        playerInteraction.HandleUVLightInteraction(this);
    }
}
