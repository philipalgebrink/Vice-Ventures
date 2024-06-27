using UnityEngine;

public class CannabisPlant : Interactable
{
    [SerializeField] private float modelChangeInterval = 30f;
    [SerializeField] private float nutrientUpdateTime = 10f;
    [SerializeField] private float lightLevelUpdateTime = 5f;
    [SerializeField] public float lightLevel = 100f;
    [SerializeField] public float WaterLevel = 35f;
    [SerializeField] public float FoodLevel = 35f;
    [SerializeField] private GameObject[] plantModels;
    [SerializeField] private Color unhealthyColor = Color.black;
    [SerializeField] private float lightCheckRadius = 5f;

    private Renderer[] renderersToColor;
    public int currentModelIndex = 0;
    private float nextModelChangeTime;
    private float nextNutrientUpdateTime;
    private float nextLightLevelUpdateTime;
    private bool isHealthy = true;
    private bool isLightNearby = false;

    void Start()
    {
        InstantiatePlantModel();
        nextModelChangeTime = Time.time + modelChangeInterval;
        nextNutrientUpdateTime = Time.time + nutrientUpdateTime;
        nextLightLevelUpdateTime = Time.time + lightLevelUpdateTime;

        renderersToColor = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        if (Time.time >= nextModelChangeTime && isHealthy && currentModelIndex < 4)
        {
            ChangeToNextModel();
            nextModelChangeTime = Time.time + modelChangeInterval;
        }

        if (WaterLevel <= 0 || FoodLevel <= 0 || WaterLevel >= 150 || FoodLevel >= 135)
        {
            isHealthy = false;
            SetPlantColor(unhealthyColor);
            return;
        }

        CheckForUVLights();

        if (Time.time >= nextLightLevelUpdateTime)
        {
            if (isLightNearby)
            {
                lightLevel = 100f;
            }
            else
            {
                lightLevel -= 5f;
                if (lightLevel <= 0)
                {
                    lightLevel = 0;
                    isHealthy = false;
                    SetPlantColor(unhealthyColor);
                    return;
                }
            }
            nextLightLevelUpdateTime = Time.time + lightLevelUpdateTime;
        }

        if (Time.time >= nextNutrientUpdateTime)
        {
            WaterLevel -= 10f;
            FoodLevel -= 5f;
            nextNutrientUpdateTime = Time.time + nutrientUpdateTime;
        }
    }

    void InstantiatePlantModel()
    {
        if (plantModels == null || plantModels.Length == 0)
        {
            Debug.LogError("[Vice] CannabisPlant: plantModels array is not initialized or empty.");
            return;
        }

        GameObject initialModel = Instantiate(plantModels[currentModelIndex], transform.position, Quaternion.identity);
        initialModel.transform.parent = transform;
    }

    void ChangeToNextModel()
    {
        if (currentModelIndex < 4)
        {
            currentModelIndex++;
        }
        else
        {
            return;
        }

        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        GameObject nextModel = Instantiate(plantModels[currentModelIndex], transform.position, Quaternion.identity);
        nextModel.transform.parent = transform;
    }

    void SetPlantColor(Color color)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = color;
            }
        }
    }

    void CheckForUVLights()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, lightCheckRadius);
        isLightNearby = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("UVLight"))
            {
                isLightNearby = true;
                break;
            }
        }
    }

    public override void Interact(PlayerInteraction playerInteraction)
    {
        // Leave this empty; actual interaction logic is handled in PlayerInteraction. But this is needed cause unity is dumb.
    }

    // Specific interaction methods

    public void WaterPlant(PlayerInteraction playerInteraction)
    {
        WaterLevel += 20f;
        playerInteraction.inventoryManager.RemoveItemInHand();
    }

    public void FeedPlant(PlayerInteraction playerInteraction)
    {
        FoodLevel += 15f;
        playerInteraction.inventoryManager.RemoveItemInHand();
    }

    public void HarvestCannabis(PlayerInteraction playerInteraction)
    {
        if (currentModelIndex == 4)
        {
            playerInteraction.playerInventory.AddItem("Unpacked Cannabis", 1);
            playerInteraction.inventoryManager.RemoveItemInHand();
            Destroy(gameObject);
        }
    }
}