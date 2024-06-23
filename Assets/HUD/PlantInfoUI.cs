using UnityEngine;
using TMPro;

public class PlantInfoUI : MonoBehaviour
{
    public float raycastDistance = 10f;
    public TextMeshProUGUI uvLevelText;
    public TextMeshProUGUI waterLevelText;
    public TextMeshProUGUI foodLevelText;
    public GameObject plantInfoCanvas;
    public string plantTag = "CannabisPlant"; // Tag used to identify cannabis plants

    private void Start()
    {
        // Initially hide the UI
        plantInfoCanvas.SetActive(false);
    }

    void Update()
    {
        UpdatePlantInfoUI();
    }

    void UpdatePlantInfoUI()
    {
        // Raycast from the center of the screen (where crosshair is)
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag(plantTag))
            {
                CannabisPlant plant = hit.collider.GetComponent<CannabisPlant>();

                if (plant != null)
                {
                    plantInfoCanvas.SetActive(true);
                    uvLevelText.text = "UV Level: " + plant.lightLevel;
                    waterLevelText.text = "Water Level: " + plant.WaterLevel;
                    foodLevelText.text = "Food Level: " + plant.FoodLevel;
                }
            }
            else
            {
                plantInfoCanvas.SetActive(false);
            }
        }
        else
        {
            plantInfoCanvas.SetActive(false);
        }
    }
}