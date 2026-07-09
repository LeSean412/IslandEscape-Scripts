using UnityEngine;
using UnityEngine.UI;

public class PlayerSurvival : MonoBehaviour
{
    public float maxHunger = 100f;
    public float maxThirst = 100f;

    public float currentHunger;
    public float currentThirst;

    public float hungerDrainRate = 0.2f;
    public float thirstDrainRate = 0.4f;

    public Slider hungerSlider;
    public Slider thirstSlider;
    private PlayerHealth playerHealth;

    void Start()
    {
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        currentHunger -= hungerDrainRate * Time.deltaTime;
        currentThirst -= thirstDrainRate * Time.deltaTime;

        if (currentHunger <= 0 || currentThirst <= 0)
        {
            currentHunger = Mathf.Max(currentHunger, 0);
            currentThirst = Mathf.Max(currentThirst, 0);
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1f * Time.deltaTime); // Starvation damage
            }
        }

        UpdateUISliders();
    }

    public void ConsumeFood(float amount)
    {
        currentHunger = Mathf.Min(currentHunger + amount, maxHunger);
        UpdateUISliders();
        Debug.Log("Consumed food. Character reacts: Yum!");
    }

    public void ConsumeWater(float amount)
    {
        currentThirst = Mathf.Min(currentThirst + amount, maxThirst);
        UpdateUISliders();
        Debug.Log("Consumed refreshing water.");
    }

    void UpdateUISliders()
    {
        if (hungerSlider != null) hungerSlider.value = currentHunger / maxHunger;
        if (thirstSlider != null) thirstSlider.value = currentThirst / maxThirst;
    }
}