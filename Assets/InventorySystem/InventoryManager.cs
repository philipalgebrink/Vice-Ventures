using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab; // Drag your InventorySlot prefab here in the Inspector
    public Transform inventoryPanel; // Drag your Inventory Panel here in the Inspector
    public PlayerInventory playerInventory; // Reference to your PlayerInventory script

    private GameObject[] inventorySlots = new GameObject[30]; // Array to hold all inventory slots
    public bool isInventoryOpen = false;
    public GameObject crosshairObject;

    void Start()
    {
        // Hide cursor at start
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
            // Example of adding an item using the new method
            playerInventory.AddItem("Cannabis Seed", 1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            // Example of adding an item using the new method
            playerInventory.AddItem("Unpacked Cannabis", 1);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            // Example of adding an item using the new method
            playerInventory.AddItem("Packed Cannabis", 1);
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
        // Get the RectTransform of the InventoryPanel
        RectTransform panelRectTransform = inventoryPanel.GetComponent<RectTransform>();

        // Define slot size and padding (adjust these values as needed)
        Vector2 originalSlotSize = new Vector2(200f, 100f); // Original slot size
        Vector2 slotSize = originalSlotSize * 1.5f; // Adjusted slot size for 1.5 scale
        Vector2 originalSlotPadding = new Vector2(2f, 2f); // Original padding between slots
        Vector2 slotPadding = originalSlotPadding * 1.5f; // Adjusted padding for 1.5 scale

        // Number of columns and rows for the grid (adjust as needed)
        int columns = 6; // Example: 6 columns
        int rows = 5; // Example: 5 rows

        // Calculate total width and height of the grid
        float gridWidth = columns * (slotSize.x + slotPadding.x) - slotPadding.x;
        float gridHeight = rows * (slotSize.y + slotPadding.y) - slotPadding.y;

        // Calculate initial position of the top-left slot to center the grid
        Vector2 panelSize = panelRectTransform.rect.size;
        Vector2 initialPosition = new Vector2(
            -gridWidth / 2f + slotSize.x / 2f, // Adjusted to center horizontally
            gridHeight / 2f - slotSize.y / 2f   // Adjusted to center vertically
        );

        // Instantiate all inventory slots in a grid
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Calculate row and column index
            int row = i / columns;
            int col = i % columns;

            // Instantiate the inventory slot prefab
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);

            // Calculate position for this slot
            float x = initialPosition.x + col * (slotSize.x + slotPadding.x);
            float y = initialPosition.y - row * (slotSize.y + slotPadding.y);
            Vector2 slotPosition = new Vector2(x, y);

            // Set anchored position relative to panel
            RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
            slotRectTransform.anchoredPosition = slotPosition;

            // Set default alpha to 0
            Image iconImage = slot.transform.Find("Icon").GetComponent<Image>();
            Color color = iconImage.color;
            color.a = 0f;
            iconImage.color = color;

            inventorySlots[i] = slot;
        }
    }

    void InitializeInventoryUI()
    {
        // Set all slots to their initial state
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            UpdateInventorySlot(i, null);
        }
    }

    void PopulateInventoryUI()
    {
        for (int i = 0; i < playerInventory.inventory.Count; i++)
        {
            InventoryItem item = playerInventory.inventory[i];
            UpdateInventorySlot(i, item);
        }
    }

    public void AddItemToInventory(InventoryItem newItem)
    {
        // Find the first available slot and update it with the new item
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
        Image iconImage = slot.transform.Find("Icon").GetComponent<Image>();
        Image circleImage = slot.transform.Find("Circle").GetComponent<Image>();

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

                Color circleColor = circleImage.color;
                circleColor.a = 1f;
                circleImage.color = circleColor;
                circleImage.enabled = true;
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
        }
    }

    void ToggleInventoryPanel()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.gameObject.SetActive(isInventoryOpen);

        // Toggle cursor visibility based on inventory open state
        Cursor.visible = isInventoryOpen;

        // Toggle crosshair visibility based on inventory open state
        if (crosshairObject != null)
        {
            crosshairObject.SetActive(!isInventoryOpen);
        }
    }
}
