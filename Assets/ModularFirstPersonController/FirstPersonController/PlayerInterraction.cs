using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerInventory playerInventory; // Reference to the PlayerInventory script
    public GameObject cannabisPlantPrefab; // Reference to the CannabisPlant prefab
    public GameObject[] plantModels; // Array to hold different plant models
    public float modelChangeInterval = 5f; // Interval between model changes

    void Update()
    {
        // Example interaction logic to spawn the cannabis plant from inventory
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Replace this with your actual detection and interaction logic for the pot
            GameObject potObject = DetectPot(); // Implement DetectPot to get the pot GameObject

            if (potObject != null && playerInventory.HasItem("Cannabis Seed"))
            {
                // Get the position of the pot
                Vector3 spawnPosition = potObject.transform.position;

                // Remove the pot from the scene
                Destroy(potObject);

                // Spawn the cannabis plant prefab at the pot's position
                SpawnCannabisPlant(spawnPosition);

                // Remove one cannabis seed from inventory
                playerInventory.RemoveItem("Cannabis Seed", 1);
            }
        }
    }

    // Example method to detect the pot object (replace with your actual detection logic)
    GameObject DetectPot()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5f))
        {
            if (hit.collider.CompareTag("Pot")) // Adjust with your tag or layer for pots
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    void SpawnCannabisPlant(Vector3 spawnPosition)
    {
        // Instantiate the cannabis plant prefab at the specified position
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
}
