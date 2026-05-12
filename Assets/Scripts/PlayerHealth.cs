using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] int maxHealth = 10;
    [SerializeField] Image[] healthBars;
    int currentHealth;

    void Awake()
    {
        maxHealth = Mathf.Min(maxHealth, healthBars.Length);
        currentHealth = maxHealth;
        AdjustHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        AdjustHealthUI();
        
        Debug.Log("Player health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead!");
        }
    }

    public int GetHealthAmount()
    {
        return currentHealth;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        AdjustHealthUI();
    }

    void AdjustHealthUI()
    {
        for(int i = 0; i < healthBars.Length; i++) {
            if(i < currentHealth){
                healthBars[i].gameObject.SetActive(true);
            } else {
                healthBars[i].gameObject.SetActive(false);
            }
        } 
    }
}