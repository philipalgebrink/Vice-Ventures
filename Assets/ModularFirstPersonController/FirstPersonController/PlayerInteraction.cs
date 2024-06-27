using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public PlayerInventory playerInventory;
    private bool isEPressed = false;
    private GameObject previewPot;
    private bool isPreviewActive = false;
    public float raycastDistance = 10f;
    public LayerMask placementLayerMask;
    public float placementHeightOffset;
    private Camera playerCamera;

    void Start()
    {
        GameObject previewPotPrefab = Resources.Load<GameObject>("Items/PotPreview");

        if (previewPotPrefab != null)
        {
            previewPot = Instantiate(previewPotPrefab);
            previewPot.SetActive(false);
        }
        else
        {
            Debug.Log("[Vice] PotPreview prefab not found in Resources/Items folder.");
        }

        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("[Vice] MainCamera not found. Ensure the camera is tagged as MainCamera.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isEPressed)
        {
            isEPressed = true;
            InteractWithObject();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isEPressed = false;
        }

        UpdatePotPreview();
    }

    public void InteractWithObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                Debug.Log($"[Vice] Interacting with: {hit.collider.gameObject.name}");
                interactable.Interact(this); // Calls the Interact method of the interactable object
            }
            else
            {
                Debug.Log("[Vice] No interactable object hit.");
                if (inventoryManager.CurrentItemInHand != null && inventoryManager.CurrentItemInHand.itemName == "Pot")
                {
                    PlacePotOnGround();
                }
                else if (inventoryManager.CurrentItemInHand != null && inventoryManager.CurrentItemInHand.itemName == "UV Light")
                {
                    PlaceUVLight();
                }
            }
        }
    }

    // CannabisPlant Interactions
    public void HandleCannabisPlantInteraction(CannabisPlant cannabisPlant)
    {
        InventoryItem itemInHand = inventoryManager.CurrentItemInHand;

        if (itemInHand != null)
        {
            switch (itemInHand.itemName)
            {
                case "Water":
                    cannabisPlant.WaterPlant(this);
                    break;
                case "PlantVital":
                    cannabisPlant.FeedPlant(this);
                    break;
                case "Scissors":
                    cannabisPlant.HarvestCannabis(this);
                    break;
                default:
                    Debug.Log("[Vice] No specific action defined for item: " + itemInHand.itemName);
                    break;
            }
        }
    }

    // Pot Interactions
    public void HandlePotInteraction(Pot pot)
    {
        InventoryItem itemInHand = inventoryManager.CurrentItemInHand;

        if (itemInHand != null && itemInHand.itemName == "Cannabis Seed")
        {
            PlantSeed(pot);
        }
        else
        {
            Debug.Log("[Vice] No specific action defined for item: " + itemInHand.itemName);
        }
    }

    private void PlantSeed(Pot pot)
    {
        // Replace pot with cannabis plant
        GameObject cannabisPlantPrefab = Resources.Load<GameObject>("Items/CannabisPlant");
        if (cannabisPlantPrefab != null)
        {
            Vector3 potPosition = pot.transform.position;
            Quaternion potRotation = pot.transform.rotation;

            // Instantiate cannabis plant at pot's position
            Instantiate(cannabisPlantPrefab, potPosition, potRotation);

            // Remove the pot
            Destroy(pot.gameObject);

            // Remove seed from inventory
            inventoryManager.RemoveItemInHand();
        }
        else
        {
            Debug.LogError("[Vice] CannabisPlant prefab not found in Resources/Items folder.");
        }
    }

    // Table Interactions
    public void HandleTableInteraction(Table table)
    {
        InventoryItem itemInHand = inventoryManager.CurrentItemInHand;

        if (itemInHand != null)
        {
            Debug.Log($"[Vice] Trying to add {itemInHand.itemName} to the table.");
            bool added = table.AddItem(itemInHand.itemName);
            if (added)
            {
                inventoryManager.RemoveItemInHand();
                Debug.Log($"[Vice] {itemInHand.itemName} added to the table.");
            }
            else
            {
                Debug.Log($"[Vice] Table already has an {itemInHand.itemName}.");
            }

            if (table.CanCombine())
            {
                CombineItems(table);
            }
        }
    }

    private void CombineItems(Table table)
    {
        playerInventory.AddItem("Packed Cannabis", 1);
        table.ResetTable();
        Debug.Log("[Vice] Packed Cannabis created.");
    }

    // UVLight Interactions
    public void HandleUVLightInteraction(UVLight uvlight)
    {
        // Add functions for UVLight here.
    }

    void UpdatePotPreview()
    {
        InventoryItem itemInHand = inventoryManager.CurrentItemInHand;

        if (itemInHand != null && itemInHand.itemName == "Pot")
        {
            if (previewPot != null && !isPreviewActive)
            {
                previewPot.SetActive(true);
                SetPreviewAlpha(previewPot, 0.5f);
                isPreviewActive = true;
            }

            if (previewPot != null)
            {
                Vector3 placementPosition;
                if (TryGetPlacementPosition(out placementPosition))
                {
                    placementPosition.y += placementHeightOffset;
                    previewPot.transform.position = placementPosition;
                }
                else
                {
                    previewPot.transform.position = transform.position + transform.forward * 2;
                }
            }
        }
        else
        {
            if (previewPot != null && isPreviewActive)
            {
                previewPot.SetActive(false);
                isPreviewActive = false;
            }
        }
    }

    bool TryGetPlacementPosition(out Vector3 placementPosition)
    {
        placementPosition = Vector3.zero;
        if (playerCamera == null)
        {
            Debug.LogError("[Vice] PlayerCamera is not assigned.");
            return false;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastDistance, placementLayerMask))
        {
            placementPosition = hit.point;
            return true;
        }
        return false;
    }

    void SetPreviewAlpha(GameObject preview, float alpha)
    {
        Renderer[] renderers = preview.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                material.SetFloat("_Mode", 3);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                Color color = material.color;
                color.a = alpha;
                material.color = color;
            }
        }
    }

    public void PlacePotOnGround()
    {
        InventoryItem itemInHand = inventoryManager.CurrentItemInHand;

        if (itemInHand != null && itemInHand.itemName == "Pot")
        {
            GameObject potPrefab = Resources.Load<GameObject>("Items/Pot");

            if (potPrefab != null)
            {
                Vector3 placementPosition;
                if (TryGetPlacementPosition(out placementPosition))
                {
                    placementPosition.y += placementHeightOffset;
                    Instantiate(potPrefab, placementPosition, Quaternion.identity);

                    playerInventory.RemoveItem(itemInHand.itemName, 1);
                    inventoryManager.CurrentItemInHand = null;

                    if (previewPot != null)
                    {
                        previewPot.SetActive(false);
                        isPreviewActive = false;
                    }
                }
            }
        }
    }

    public void PlaceUVLight()
    {
        InventoryItem itemInHand = inventoryManager.CurrentItemInHand;

        if (itemInHand != null && itemInHand.itemName == "UV Light")
        {
            GameObject uvlightPrefab = Resources.Load<GameObject>("Items/UVLight");

            if (uvlightPrefab != null)
            {
                Vector3 placementPosition;
                if (TryGetPlacementPosition(out placementPosition))
                {
                    placementPosition.y += placementHeightOffset;
                    GameObject instance = Instantiate(uvlightPrefab, placementPosition, Quaternion.identity);

                    Light lightComponent = instance.AddComponent<Light>();
                    lightComponent.type = LightType.Point;
                    lightComponent.range = 10f;
                    lightComponent.intensity = 3f;
                    lightComponent.color = new Color(128f / 255f, 0f, 128f / 255f);

                    instance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                    BoxCollider collider = instance.AddComponent<BoxCollider>();
                    collider.isTrigger = true;

                    instance.tag = "UVLight";

                    playerInventory.RemoveItem(itemInHand.itemName, 1);
                    inventoryManager.CurrentItemInHand = null;
                }
            }
        }
    }
}