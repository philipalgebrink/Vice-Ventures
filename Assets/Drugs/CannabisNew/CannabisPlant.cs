using UnityEngine;

public class CannabisPlant : MonoBehaviour
{
    [SerializeField] private float modelChangeInterval = 30f; // Interval between model changes
    [SerializeField] private float nutrientUpdateTime = 10f; // Interval between food/water level changes
    [SerializeField] private float lightLevelUpdateTime = 5f; // Interval between light level changes
    [SerializeField] public float WaterLevel = 35f; // Water Level
    [SerializeField] public float FoodLevel = 35f; // Food Level
    [SerializeField] private GameObject[] plantModels; // Array to hold different plant models
    [SerializeField] private Color unhealthyColor = Color.black; // Color when plant is unhealthy
    [SerializeField] private float lightCheckRadius = 5f; // Radius to check for UV lights

    private Renderer[] renderersToColor; // Array to hold renderers to be colored
    public int currentModelIndex = 0; // Index of current model
    private float nextModelChangeTime; // Time to change to next model
    private float nextNutrientUpdateTime;
    private float nextLightLevelUpdateTime;
    private bool isHealthy = true; // Flag to track plant health
    private float lightLevel = 100f; // Light Level
    private bool isLightNearby = false; // Flag to check if UV light is nearby

    // Inventory and UI references
    private PlayerInventory playerInventory;
    private InventoryManager inventoryManager;

    void Start()
    {
        // Initialize the first model and set up next model change time
        InstantiatePlantModel();
        nextModelChangeTime = Time.time + modelChangeInterval;
        nextNutrientUpdateTime = Time.time + nutrientUpdateTime;
        nextLightLevelUpdateTime = Time.time + lightLevelUpdateTime;

        // Example: Assign renderers to be colored (replace with actual logic)
        renderersToColor = GetComponentsInChildren<Renderer>(); // Adjust this to fit your actual setup
    }

    void Update()
    {
        // Check if it's time to change the plant model
        if (Time.time >= nextModelChangeTime && isHealthy && currentModelIndex < 4)
        {
            ChangeToNextModel();
            nextModelChangeTime = Time.time + modelChangeInterval; // Reset the timer
        }

        // Check nutrient levels
        if (WaterLevel <= 0 || FoodLevel <= 0)
        {
            isHealthy = false;
            SetPlantColor(unhealthyColor);
            return; // Stop updating nutrient levels if unhealthy
        }

        // Check for nearby UV lights
        CheckForUVLights();

        // Update light level every 5 seconds
        if (Time.time >= nextLightLevelUpdateTime)
        {
            if (isLightNearby)
            {
                Debug.Log("Light is nearby");
                lightLevel = 100f;
            }
            else
            {
                Debug.Log("lightLevel" + lightLevel);
                lightLevel -= 5f;
                if (lightLevel <= 0)
                {
                    lightLevel = 0;
                    isHealthy = false;
                    SetPlantColor(unhealthyColor);
                    return; // Stop updating nutrient levels if unhealthy
                }
            }
            nextLightLevelUpdateTime = Time.time + lightLevelUpdateTime;
        }

        // Update nutrient levels
        if (Time.time >= nextNutrientUpdateTime)
        {
            WaterLevel -= 10f;
            FoodLevel -= 5f;
            nextNutrientUpdateTime = Time.time + nutrientUpdateTime;
        }
    }

    public void Initialize(PlayerInventory inventory, InventoryManager manager)
    {
        // Set inventory and UI references
        playerInventory = inventory;
        inventoryManager = manager;
    }

    void InstantiatePlantModel()
    {
        // Check if plantModels array is null or empty
        if (plantModels == null || plantModels.Length == 0)
        {
            Debug.LogError("[Vice] CannabisPlant: plantModels array is not initialized or empty.");
            return;
        }

        // Spawn initial plant model at the position of this GameObject
        GameObject initialModel = Instantiate(plantModels[currentModelIndex], transform.position, Quaternion.identity);
        initialModel.transform.parent = transform; // Set as child of this GameObject
    }

    void ChangeToNextModel()
    {
        // Increment index for next model
        if (currentModelIndex < 4)
        {
            currentModelIndex++;
        }
        else
        {
            return; // Return if fully grown
        }

        // Check if there is a child object to destroy
        if (transform.childCount > 0)
        {
            // Destroy current model
            Destroy(transform.GetChild(0).gameObject);
        }

        // Spawn next plant model at the position of this GameObject
        GameObject nextModel = Instantiate(plantModels[currentModelIndex], transform.position, Quaternion.identity);
        nextModel.transform.parent = transform; // Set as child of this GameObject
    }

    void SetPlantColor(Color color)
    {
        // Get all renderers in the GameObject and its children
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            // Get all materials used by the renderer
            Material[] materials = renderer.materials;

            // Set the color of each material
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = color;
            }
        }
    }

    void CheckForUVLights()
    {
        // Check for nearby UV lights within the specified radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, lightCheckRadius);
        isLightNearby = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("UVLight"))
            {
                Debug.Log("Light is nearby: " + hitCollider.gameObject.name);
                isLightNearby = true;
                break;
            }
        }

        if (!isLightNearby)
        {
            Debug.Log("No UV light nearby");
        }
    }
}