using UnityEngine;

public class CannabisSpawnController : MonoBehaviour
{
    public GameObject cannabisPlantPrefab; // Reference to the CannabisPlant prefab
    public GameObject[] plantModels; // Array to hold different plant models
    public float modelChangeInterval = 60f; // Interval between model changes

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
        // Maximum distance from camera to spawn the plant
        float maxSpawnDistance = 3f;

        // Calculate spawn position in front of the camera
        Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * 2f;
        
        // Raycast to ensure the plant spawns on a suitable surface within the distance limit
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Ground"); // Define layers you want to spawn on
        
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, maxSpawnDistance, layerMask))
        {
            // Check if hit point is within the allowed distance
            if (hit.distance <= maxSpawnDistance)
            {
                spawnPosition = hit.point + Vector3.up * 0.2f; // Offset spawn position upwards by 0.2 units

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
                Debug.Log("Spawn distance exceeds maximum allowed distance.");
            }
        }
        else
        {
            Debug.Log("Cannot spawn plant here. Aim at a surface within distance.");
        }
    }
}
