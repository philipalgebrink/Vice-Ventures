using UnityEngine;

public class CannabisPlant : MonoBehaviour
{
    [SerializeField] private float modelChangeInterval = 30f; // Interval between model changes
    [SerializeField] private GameObject[] plantModels; // Array to hold different plant models

    private int currentModelIndex = 0; // Index of current model
    private float nextModelChangeTime; // Time to change to next model

    void Start()
    {
        // Initialize the first model and set up next model change time
        InstantiatePlantModel();
        nextModelChangeTime = Time.time + modelChangeInterval;
    }

    void Update()
    {
        // Check if it's time to change the plant model
        if (Time.time >= nextModelChangeTime)
        {
            ChangeToNextModel();
            nextModelChangeTime = Time.time + modelChangeInterval; // Reset the timer
        }
    }

    public void Initialize(GameObject[] models, float interval)
    {
        if (models == null || models.Length == 0)
        {
            Debug.LogError("[Vice] CannabisPlant: No plant models provided.");
            return;
        }

        plantModels = models;
        modelChangeInterval = interval;
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
        currentModelIndex++;

        // If we exceed the bounds of the array, reset to 0
        if (currentModelIndex >= plantModels.Length)
        {
            currentModelIndex = 0; // Reset index to 0
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
        Debug.Log($"[Vice] Spawned plant model: {plantModels[currentModelIndex].name} at index {currentModelIndex}");
    }
}
