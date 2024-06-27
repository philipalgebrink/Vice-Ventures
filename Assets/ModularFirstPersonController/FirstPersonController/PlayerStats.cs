using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    // Money variables
    public float money = 1000;
    public TMP_Text moneyTextMesh;

    // Health variables
    public float health = 100f;

    // Hunger variables
    public float hunger = 100f; // Start at 100
    public float maxHunger = 100f;
    public float hungerDecreaseInterval = 60f; // Decrease hunger every 60 seconds
    private float nextHungerDecreaseTime;

    // Thirst variables
    public float thirst = 100f; // Start at 100
    public float maxThirst = 100f;
    public float thirstDecreaseInterval = 60f; // Decrease thirst every 60 seconds
    private float nextThirstDecreaseTime;

    public TMP_Text hungerTextMesh;
    public TMP_Text thirstTextMesh;

    void UpdateUI()
    {
        moneyTextMesh.text = money.ToString();
        hungerTextMesh.text = Mathf.Round(hunger).ToString(); // Display rounded hunger value
        thirstTextMesh.text = Mathf.Round(thirst).ToString(); // Display rounded thirst value
        // Update other text elements similarly for health, money, etc.
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();

        // Initialize next decrease times
        nextHungerDecreaseTime = Time.time + hungerDecreaseInterval;
        nextThirstDecreaseTime = Time.time + thirstDecreaseInterval;
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease hunger and thirst over time
        DecreaseHungerOverTime();
        DecreaseThirstOverTime();

        // Update UI to reflect changes
        UpdateUI();
    }

    void DecreaseHungerOverTime()
    {
        if (Time.time >= nextHungerDecreaseTime)
        {
            hunger -= 1f; // Decrease hunger by 1 unit
            hunger = Mathf.Clamp(hunger, 0f, maxHunger); // Ensure hunger stays between 0 and maxHunger
            nextHungerDecreaseTime += hungerDecreaseInterval; // Set next decrease time
        }
    }

    void DecreaseThirstOverTime()
    {
        if (Time.time >= nextThirstDecreaseTime)
        {
            thirst -= 1f; // Decrease thirst by 1 unit
            thirst = Mathf.Clamp(thirst, 0f, maxThirst); // Ensure thirst stays between 0 and maxThirst
            nextThirstDecreaseTime += thirstDecreaseInterval; // Set next decrease time
        }
    }
}
