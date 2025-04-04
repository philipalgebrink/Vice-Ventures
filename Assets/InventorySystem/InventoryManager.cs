using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;
    public Transform inventoryPanel;
    public PlayerInventory playerInventory;
    public GameObject player;
    public GameObject crosshairObject;

    private GameObject[] inventorySlots = new GameObject[30];
    private InventoryItem currentItemInHand;
    private HUDManager hudManager;

    public InventoryItem CurrentItemInHand
    {
        get { return currentItemInHand; }
        set { currentItemInHand = value; }
    }
    public bool isInventoryOpen = false;

    public event Action OnInventoryUIChanged;

    void Start()
    {
        Cursor.visible = false;
        CreateInventorySlots();
        InitializeInventoryUI();

        if (playerInventory == null)
        {
            Debug.LogError("[Vice] PlayerInventory is not assigned.");
            return;
        }

        playerInventory.OnInventoryChanged += PopulateInventoryUI;
        inventoryPanel.gameObject.SetActive(false);

        // Find and assign HUDManager
        hudManager = FindObjectOfType<HUDManager>();
        if (hudManager == null)
        {
            Debug.LogError("[Vice] HUDManager not found in the scene.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("[Vice] Are we adding cannabis seed?");
            playerInventory.AddItem("Cannabis Seed", 5);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("[Vice] Are we adding pot?");
            playerInventory.AddItem("Pot", 5);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("[Vice] Are we adding water?");
            playerInventory.AddItem("Water", 5);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("[Vice] Are we adding Unpacked Cannabis?");
            playerInventory.AddItem("Unpacked Cannabis", 5);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("[Vice] Are we adding UV Light?");
            playerInventory.AddItem("UV Light", 1);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("[Vice] Are we adding Scissors?");
            playerInventory.AddItem("Scissors", 1);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("[Vice] Are we adding Empty Zipbag?");
            playerInventory.AddItem("Empty Zipbag", 1);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryPanel();
        }

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

        Vector2 originalSlotSize = new Vector2(200f, 100f);
        Vector2 slotSize = originalSlotSize * 1.5f;
        Vector2 originalSlotPadding = new Vector2(2f, 2f);
        Vector2 slotPadding = originalSlotPadding * 1.5f;

        int columns = 6;
        int rows = 5;

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
                Debug.LogError("[Vice] Icon Image component not found in slot prefab.");
            }
            Color color = iconImage.color;
            color.a = 0f;
            iconImage.color = color;

            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            if (inventorySlot == null)
            {
                Debug.LogError("[Vice] InventorySlot component not found in slot prefab.");
                return;
            }
            inventorySlot.inventoryManager = this;

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
        if (playerInventory == null)
        {
            Debug.LogError("[Vice] PlayerInventory is not assigned.");
            return;
        }

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

        Debug.Log("[Vice] Inventory is full!");
    }

    void UpdateInventorySlot(int slotIndex, InventoryItem item)
    {
        GameObject slot = inventorySlots[slotIndex];
        slot.SetActive(true);

        TextMeshProUGUI nameText = slot.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descriptionText = slot.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI quantityText = slot.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();
        Image iconImage = slot.transform.Find("Icon").GetComponent<Image>();
        Image circleImage = slot.transform.Find("Circle").GetComponent<Image>();

        if (nameText == null || descriptionText == null || quantityText == null || iconImage == null || circleImage == null)
        {
            Debug.LogError("[Vice] One or more UI components are missing in the slot prefab.");
            return;
        }

        InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
        if (inventorySlot == null)
        {
            Debug.LogError("[Vice] InventorySlot component not found in slot prefab.");
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
                color.a = 1f;
                iconImage.color = color;
                iconImage.enabled = true;

                Color circleColor = circleImage.color;
                circleColor.a = 1f;
                circleImage.color = circleColor;
                circleImage.enabled = true;
            }
            else
            {
                Color color = iconImage.color;
                color.a = 0f;
                iconImage.color = color;
                iconImage.enabled = false;

                Color circleColor = circleImage.color;
                circleColor.a = 0f;
                circleImage.color = circleColor;
                circleImage.enabled = false;
            }

            EventTrigger eventTrigger = iconImage.gameObject.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = iconImage.gameObject.AddComponent<EventTrigger>();
            }

            eventTrigger.triggers.Clear();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { OnSlotClicked(item); });
            eventTrigger.triggers.Add(entry);
        }
        else
        {
            nameText.text = "";
            descriptionText.text = "";
            quantityText.text = "";
            Color color = iconImage.color;
            color.a = 0f;
            iconImage.color = color;
            iconImage.enabled = false;

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

    public InventoryItem GetCurrentItemInHand()
    {
        return currentItemInHand;
    }

    public void UseItemInHand()
    {
        InventoryItem itemInHand = currentItemInHand;

        if (itemInHand != null)
        {
            Debug.Log("[Vice] Attempting to use item in hand: " + itemInHand.itemName);
            PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
            if (playerInteraction != null)
            {
                playerInteraction.InteractWithObject();
            }
            else
            {
                Debug.Log("[Vice] PlayerInteraction script not found.");
            }
        }
        else
        {
            Debug.Log("[Vice] No item in hand to use.");
        }
    }

    public void RemoveItemInHand()
    {
        if (currentItemInHand != null)
        {
            playerInventory.RemoveItem(currentItemInHand.itemName, 1);
            if (currentItemInHand.quantity <= 0)
            {
                currentItemInHand = null;
            }
            OnInventoryUIChanged?.Invoke();
        }
    }

    public void OnSlotClicked(InventoryItem item)
    {
        if (currentItemInHand == item)
        {
            CurrentItemInHand = null;
        }
        else
        {
            PutItemInHand(item);
            Debug.Log("[Vice] Item picked up: " + currentItemInHand.itemName);
        }

        // Update HUDManager UI
        hudManager.UpdateItemInHandUI();
    }

    public void PutItemInHand(InventoryItem item)
    {
        currentItemInHand = item;
        UpdateInventorySlotUI(item); // Update inventory UI to indicate item is in hand
        hudManager.UpdateItemInHandUI(); // Update HUDManager UI
    }

    public Sprite GetItemInHandSprite()
    {
        if (currentItemInHand != null)
        {
            return currentItemInHand.icon; // Return the icon sprite of the item in hand
        }
        else
        {
            return null; // Or return a default sprite indicating no item in hand
        }
    }

    void UpdateInventorySlotUI(InventoryItem item)
    {
        // Implement logic to visually update the inventory slot UI when item is in hand
        // This can include highlighting or changing the appearance of the UI element
    }
}