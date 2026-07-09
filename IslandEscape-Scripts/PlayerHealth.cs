using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.value = CalculateHealthPercentage();
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            // Trigger player death logic here
            Debug.Log("Player is dead!");
        }

        if (healthSlider != null)
        {
            healthSlider.value = CalculateHealthPercentage();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (healthSlider != null)
        {
            healthSlider.value = CalculateHealthPercentage();
        }
    }

    private float CalculateHealthPercentage()
    {
        return currentHealth;
            }
}