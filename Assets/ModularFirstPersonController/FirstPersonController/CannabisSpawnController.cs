using UnityEngine;

public class CannabisSpawnController : MonoBehaviour
{
    public GameObject cannabisPlantPrefab; // Reference to the CannabisPlant prefab
    public GameObject[] plantModels; // Array to hold different plant models
    public float modelChangeInterval = 5f; // Interval between model changes

    // Reference to the main camera
    private Camera mainCamera;

    void Start()
    {
        // Assuming the main camera is tagged as "MainCamera"
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Example: spawn the plant when E key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnPlant();
        }
    }

    void SpawnPlant()
    {
        // Calculate spawn position in front of the camera
        Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * 2f;
        
        // Raycast to ensure the plant spawns on a suitable surface (e.g., ground)
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Ground"); // Define layers you want to spawn on
        
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            spawnPosition = hit.point + Vector3.up * 0.2f; // Offset spawn position upwards by 0.5 units

            // Spawn the cannabis plant prefab
            GameObject newPlant = Instantiate(cannabisPlantPrefab, spawnPosition, Quaternion.identity);
            
            // Get the CannabisPlant component from the spawned plant
            CannabisPlant plantScript = newPlant.GetComponent<CannabisPlant>();

            // Initialize the CannabisPlant script
            if (plantScript != null)
            {
                plantScript.Initialize(plantModels, modelChangeInterval);
            }
            else
            {
                Debug.LogError("CannabisPlant component not found on prefab.");
            }
        }
        else
        {
            Debug.Log("Cannot spawn plant here. Aim at a floor or table.");
        }
    }
}
