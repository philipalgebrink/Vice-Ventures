using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab; // Drag your InventorySlot prefab here in the Inspector
    public Transform inventoryPanel; // Drag your Inventory Panel here in the Inspector
    public PlayerInventory playerInventory; // Reference to your PlayerInventory script
    public GameObject player; // Reference to the player

    private GameObject[] inventorySlots = new GameObject[30]; // Array to hold all inventory slots
    public bool isInventoryOpen = false;
    public GameObject crosshairObject;

    void Start()
    {
        Cursor.visible = false;

        CreateInventorySlots(); // Create all inventory slots initially
        InitializeInventoryUI(); // Initialize all slots to the correct state
        playerInventory.OnInventoryChanged += PopulateInventoryUI; // Subscribe to inventory changes
        inventoryPanel.gameObject.SetActive(false); // Initially hide inventory panel
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerInventory.AddItem("Cannabis Seed", 5);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryPanel();
        }

        // Lock cursor position when inventory panel is open
        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void CreateInventorySlots()
    {
        RectTransform panelRectTransform = inventoryPanel.GetComponent<RectTransform>();

        Vector2 originalSlotSize = new Vector2(200f, 100f); // Original slot size
        Vector2 slotSize = originalSlotSize * 1.5f; // Adjusted slot size for 1.5 scale
        Vector2 originalSlotPadding = new Vector2(2f, 2f); // Original padding between slots
        Vector2 slotPadding = originalSlotPadding * 1.5f; // Adjusted padding for 1.5 scale

        int columns = 6; // Example: 6 columns
        int rows = 5; // Example: 5 rows

        float gridWidth = columns * (slotSize.x + slotPadding.x) - slotPadding.x;
        float gridHeight = rows * (slotSize.y + slotPadding.y) - slotPadding.y;

        Vector2 panelSize = panelRectTransform.rect.size;
        Vector2 initialPosition = new Vector2(
            -gridWidth / 2f + slotSize.x / 2f,
            gridHeight / 2f - slotSize.y / 2f
        );

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            int row = i / columns;
            int col = i % columns;

            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);

            float x = initialPosition.x + col * (slotSize.x + slotPadding.x);
            float y = initialPosition.y - row * (slotSize.y + slotPadding.y);
            Vector2 slotPosition = new Vector2(x, y);

            RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
            slotRectTransform.anchoredPosition = slotPosition;

            Image iconImage = slot.transform.Find("Icon").GetComponent<Image>();
            if (iconImage == null)
            {
                Debug.LogError("Icon Image component not found in slot prefab.");
            }
            Color color = iconImage.color;
            color.a = 0f;
            iconImage.color = color;

            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            if (inventorySlot == null)
            {
                Debug.LogError("InventorySlot component not found in slot prefab.");
            }
            inventorySlot.inventoryManager = this; // Set the InventoryManager reference dynamically

            inventorySlots[i] = slot;
        }
    }

    void InitializeInventoryUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            UpdateInventorySlot(i, null);
        }
    }

    void PopulateInventoryUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < playerInventory.inventory.Count)
            {
                InventoryItem item = playerInventory.inventory[i];
                UpdateInventorySlot(i, item);
            }
            else
            {
                UpdateInventorySlot(i, null);
            }
        }
    }

    public void AddItemToInventory(InventoryItem newItem)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (!inventorySlots[i].activeSelf)
            {
                UpdateInventorySlot(i, newItem);
                return;
            }
        }

        Debug.Log("Inventory is full!"); // Handle case where inventory is full
    }

    void UpdateInventorySlot(int slotIndex, InventoryItem item)
    {
        GameObject slot = inventorySlots[slotIndex];
        slot.SetActive(true); // Activate the slot

        TextMeshProUGUI nameText = slot.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descriptionText = slot.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI quantityText = slot.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();
        Image iconImage = slot.transform.Find("Icon").GetComponent<Image>(); // Get the Icon image component
        Image circleImage = slot.transform.Find("Circle").GetComponent<Image>(); // Get the Circle image component

        if (nameText == null || descriptionText == null || quantityText == null || iconImage == null || circleImage == null)
        {
            Debug.LogError("One or more UI components are missing in the slot prefab.");
            return;
        }

        InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
        if (inventorySlot == null)
        {
            Debug.LogError("InventorySlot component not found in slot prefab.");
            return;
        }
        inventorySlot.item = item;

        if (item != null)
        {
            nameText.text = item.itemName;
            descriptionText.text = item.description;
            quantityText.text = item.quantity.ToString();

            if (item.icon != null)
            {
                iconImage.sprite = item.icon;
                Color color = iconImage.color;
                color.a = 1f; // Set alpha to 1 (fully visible)
                iconImage.color = color;
                iconImage.enabled = true;

                // Set Circle alpha to 1
                Color circleColor = circleImage.color;
                circleColor.a = 1f;
                circleImage.color = circleColor;
                circleImage.enabled = true;
            }
            else
            {
                Color color = iconImage.color;
                color.a = 0f; // Set alpha to 0 (invisible)
                iconImage.color = color;
                iconImage.enabled = false;

                // Set Circle alpha to 0
                Color circleColor = circleImage.color;
                circleColor.a = 0f;
                circleImage.color = circleColor;
                circleImage.enabled = false;
            }
        }
        else
        {
            nameText.text = "";
            descriptionText.text = "";
            quantityText.text = "";
            Color color = iconImage.color;
            color.a = 0f; // Set alpha to 0 (invisible)
            iconImage.color = color;
            iconImage.enabled = false;

            // Set Circle alpha to 0
            Color circleColor = circleImage.color;
            circleColor.a = 0f;
            circleImage.color = circleColor;
            circleImage.enabled = false;
        }
    }

    void ToggleInventoryPanel()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.gameObject.SetActive(isInventoryOpen);

        Cursor.visible = isInventoryOpen;

        if (crosshairObject != null)
        {
            crosshairObject.SetActive(!isInventoryOpen);
        }
    }

    public void SpawnItem(InventoryItem item)
    {
        string prefabPath = "Items/" + item.itemName; // Assuming prefab names match item names including spaces
        GameObject itemPrefab = Resources.Load<GameObject>(prefabPath);

        if (itemPrefab != null)
        {
            Vector3 spawnPosition = player.transform.position + player.transform.forward * 2f; // Spawn 2 units in front of the player
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

            // Reduce the quantity of the item in the inventory
            playerInventory.RemoveItem(item.itemName, 1);
        }
        else
        {
            Debug.LogWarning("Prefab not found for item: " + item.itemName + " at path: " + prefabPath);
        }
    }
}
