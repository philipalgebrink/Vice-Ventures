using UnityEngine;

public class NpcBase : Interactable
{
    public override void Interact(PlayerInteraction playerInteraction)
    {
        Debug.LogError("[Vice] NpcBase: Interact method called. This should not happen, as the method should be overridden in derived classes.");
        throw new System.NotImplementedException();
    }
}
