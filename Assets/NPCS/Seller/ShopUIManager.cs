using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : UIManagerBase
{
    public static ShopUIManager Instance { get; private set; }

    public GameObject shopPanel;
    public Transform itemsContainer;
    public GameObject itemPrefab;
    public Button closeButton;

    private PlayerInventory playerInventory;
    private PlayerStats playerStats;

    private bool isShopOpen = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return; // Early return to avoid running the rest of Awake() on the duplicate instance
        }

        SetupUIManager();
    }

    public void SetupUIManager()
    {
        if (shopPanel == null || itemsContainer == null || itemPrefab == null || closeButton == null)
        {
            Debug.LogError("[Vice] Some references are not assigned in the ShopUIManager script.");
            return;
        }

        // Find and assign PlayerInventory and PlayerStats
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerStats = FindObjectOfType<PlayerStats>();

        if (playerInventory == null || playerStats == null)
        {
            Debug.LogError("[Vice] PlayerInventory or PlayerStats not found in the scene.");
        }

        // Setup close button
        closeButton.onClick.AddListener(CloseShop);
    }

    public override void CloseUI()
    {
        CloseShop();
    }

    private void Start()
    {
        // Ensure the shop panel is hidden at start
        CloseShop();
    }

    public void OpenShop(Seller seller)
    {
        // Look for the HUDManager object in the scene.
        HUDManager hudManager = FindObjectOfType<HUDManager>();
        hudManager.CloseAllUIs();

        isShopOpen = true;
        shopPanel.SetActive(true);
        PopulateShop(seller);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void CloseShop()
    {
        isShopOpen = false;
        shopPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public bool IsShopOpen()
    {
        return isShopOpen;
    }

    public void PopulateShop(Seller seller)
    {
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject); // Clear existing items
        }

        foreach (var item in seller.itemsForSale)
        {
            GameObject itemGO = Instantiate(itemPrefab, itemsContainer);
            ShopItemUI shopItemUI = itemGO.GetComponent<ShopItemUI>();

            if (shopItemUI != null)
            {
                shopItemUI.Setup(item, playerInventory, playerStats);
                Debug.Log($"[Vice] Setup {item.itemName} with price {item.price}.");
            }
            else
            {
                Debug.LogError("[Vice] ShopItemUI component not found on ItemPrefab.");
            }
        }
    }
}
