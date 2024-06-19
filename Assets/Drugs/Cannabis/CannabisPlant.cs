using UnityEngine;

public class CannabisPlant : MonoBehaviour
{
    private int currentModelIndex = 0; // Index of current model
    private float nextModelChangeTime; // Time to change to next model
    private GameObject[] plantModels; // Array to hold different plant models
    private float modelChangeInterval; // Interval between model changes

    public void Initialize(GameObject[] models, float interval)
    {
        if (models == null || models.Length == 0)
        {
            Debug.LogError("CannabisPlant: No plant models provided.");
            return;
        }

        plantModels = models;
        modelChangeInterval = interval;

        // Initialize the first model and set up next model change time
        InstantiatePlantModel();
        nextModelChangeTime = Time.time + modelChangeInterval;
    }

    void Update()
    {
        // Check if plantModels array is null or empty
        if (plantModels == null || plantModels.Length == 0)
        {
            Debug.LogError("CannabisPlant: plantModels array is not initialized or empty.");
            return;
        }

        // Check if it's time to change the plant model
        if (Time.time >= nextModelChangeTime)
        {
            ChangeToNextModel();
            nextModelChangeTime += modelChangeInterval;
        }
    }

    void InstantiatePlantModel()
    {
        // Check if plantModels array is null or empty
        if (plantModels == null || plantModels.Length == 0)
        {
            Debug.LogError("CannabisPlant: plantModels array is not initialized or empty.");
            return;
        }

        // Spawn initial plant model at the position of this GameObject
        GameObject initialModel = Instantiate(plantModels[currentModelIndex], transform.position, Quaternion.identity);
        initialModel.transform.parent = transform; // Set as child of this GameObject
    }

    void ChangeToNextModel()
    {
        // Check if plantModels array is null or empty
        if (plantModels == null || plantModels.Length == 0)
        {
            Debug.LogError("CannabisPlant: plantModels array is not initialized or empty.");
            return;
        }

        // If we are at the last model, do not change anymore
        if (currentModelIndex >= plantModels.Length - 1)
        {
            return; // Exit the method early
        }

        // Check if there is a child object to destroy
        if (transform.childCount > 0)
        {
            // Destroy current model
            Destroy(transform.GetChild(0).gameObject);
        }

        // Increment index for next model
        currentModelIndex++;

        // Spawn next plant model at the position of this GameObject
        GameObject nextModel = Instantiate(plantModels[currentModelIndex], transform.position, Quaternion.identity);
        nextModel.transform.parent = transform; // Set as child of this GameObject
    }
}
