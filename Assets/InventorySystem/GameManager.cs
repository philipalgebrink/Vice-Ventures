using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private InventoryItem currentItemInHand;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void AddItemToHand(InventoryItem item)
    {
        currentItemInHand = item;
        Debug.Log("[Vice] Item added to hand: " + item.itemName);
    }

}
