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
    public float placementHeightOffset; // Height offset for placement
    private Camera playerCamera;

    void Start()
    {
        // Load the PotPreview prefab from Resources/Items folder
        GameObject previewPotPrefab = Resources.Load<GameObject>("Items/PotPreview");

        if (previewPotPrefab != null)
        {
            // Instantiate the preview Pot but keep it inactive initially
            previewPot = Instantiate(previewPotPrefab);
            previewPot.SetActive(false);
        }
        else
        {
            Debug.Log("[Vice] PotPreview prefab not found in Resources/Items folder.");
        }

        // Find the PlayerCamera
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
            inventoryManager.UseItemInHand();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isEPressed = false;
        }

        UpdatePotPreview();
    }

    void UpdatePotPreview()
    {
        InventoryItem itemInHand = inventoryManager.CurrentItemInHand;

        if (itemInHand != null && itemInHand.itemName == "Pot")
        {
            if (previewPot != null && !isPreviewActive)
            {
                previewPot.SetActive(true);
                SetPreviewAlpha(previewPot, 0.5f); // Set transparency to 50%
                isPreviewActive = true;
            }

            if (previewPot != null)
            {
                // Update the preview Pot position based on the raycast hit position
                Vector3 placementPosition;
                if (TryGetPlacementPosition(out placementPosition))
                {
                    placementPosition.y += placementHeightOffset;
                    previewPot.transform.position = placementPosition;
                    Debug.Log($"[Vice] Preview pot position updated to: {placementPosition}");
                }
                else
                {
                    Debug.Log("[Vice] No valid placement position found.");
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

        // Perform raycast from the center of the screen
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastDistance, placementLayerMask))
        {
            placementPosition = hit.point;
            Debug.Log($"[Vice] Raycast hit: {hit.collider.gameObject.name} at {hit.point}");
            return true;
        }
        else
        {
            Debug.Log("[Vice] Raycast did not hit any valid objects.");
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
                // Set the rendering mode to Transparent
                material.SetFloat("_Mode", 3);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                // Set the alpha value
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
            // Load the Pot prefab from Resources/Item folder
            GameObject potPrefab = Resources.Load<GameObject>("Items/Pot");

            if (potPrefab != null)
            {
                // Get the placement position based on the raycast hit position
                Vector3 placementPosition;
                if (TryGetPlacementPosition(out placementPosition))
                {
                    placementPosition.y += placementHeightOffset;
                    // Instantiate the Pot prefab at the placement position
                    Instantiate(potPrefab, placementPosition, Quaternion.identity);
                    Debug.Log($"[Vice] Pot placed at: {placementPosition}");

                    // Remove one "Pot" from inventory
                    playerInventory.RemoveItem(itemInHand.itemName, 1);

                    // Clear item from hand
                    inventoryManager.CurrentItemInHand = null;
                    Debug.Log("[Vice] Quantity should have decreased");
                    Debug.Log("[Vice] Should not have anything in hand");

                    // Hide the preview pot
                    if (previewPot != null)
                    {
                        previewPot.SetActive(false);
                        isPreviewActive = false;
                    }
                }
                else
                {
                    Debug.Log("[Vice] No valid placement position found.");
                }
            }
            else
            {
                Debug.Log("[Vice] Pot prefab not found in Resources/Item folder.");
            }
        }
        else if (itemInHand == null)
        {
            Debug.Log("[Vice] No Pot in hand to place on ground.");
        }
        else
        {
            Debug.Log("[Vice] Item in hand is not a Pot.");
        }
    }

    public void PlaceUVLight()
    {
        InventoryItem itemInHand = inventoryManager.CurrentItemInHand;

        if (itemInHand != null && itemInHand.itemName == "UV Light")
        {
            // Load the UV Light prefab from Resources/Items folder
            GameObject uvlightPrefab = Resources.Load<GameObject>("Items/UVLight");

            if (uvlightPrefab != null)
            {
                // Instantiate the UV Light prefab at the player's position
                GameObject instance = Instantiate(uvlightPrefab, transform.position, Quaternion.identity);

                // Add and configure the Light component
                Light lightComponent = instance.AddComponent<Light>();
                lightComponent.type = LightType.Point;
                lightComponent.range = 10f;
                lightComponent.intensity = 3f;
                lightComponent.color = new Color(128f / 255f, 0f, 128f / 255f);

                // Set the scale of the instantiated object to 0.5 on all axes
                instance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                // Add a collider and set it as a trigger
                BoxCollider collider = instance.AddComponent<BoxCollider>();
                collider.isTrigger = true;

                // Set the tag to "UVLight"
                instance.tag = "UVLight";

                Debug.Log("[Vice] UV Light placed.");

                // Remove one "UV Light" from inventory
                playerInventory.RemoveItem(itemInHand.itemName, 1);

                // Clear item from hand
                inventoryManager.CurrentItemInHand = null;
                Debug.Log("[Vice] Quantity should have decreased");
                Debug.Log("[Vice] Should not have anything in hand");
            }
            else
            {
                Debug.Log("[Vice] UV Light prefab not found in Resources/Item folder.");
            }
        }
        else if (itemInHand == null)
        {
            Debug.Log("[Vice] No UV Light in hand to place on ground.");
        }
        else
        {
            Debug.Log("[Vice] Item in hand is not a UV Light.");
        }
    }
}