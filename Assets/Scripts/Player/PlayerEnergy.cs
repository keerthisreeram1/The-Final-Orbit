using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{   
    [Range(1, 10)]
    [SerializeField] int maxEnergy = 10;
    [SerializeField] Image[] energyBars;

    int currentEnergy;

    void Awake()
    {
        maxEnergy = Mathf.Min(maxEnergy, energyBars.Length);
        currentEnergy = maxEnergy;
        AdjustEnergyUI();
    }

    public void TakeDamage(int amount)
    {
        currentEnergy -= amount;
        AdjustEnergyUI();

        Debug.Log("Player Energy: " + currentEnergy);

        if (currentEnergy <= 0)
        {
            Debug.Log("Player is dead!");
        }
    }

    public int GetEnergyAmount()
    {
        return currentEnergy;
    }

    public void HealEnergy(int amount)
    {
        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + amount);
        AdjustEnergyUI();
    }

    void AdjustEnergyUI()
    {
        for(int i = 0; i < energyBars.Length; i++) {
            if(i < currentEnergy){
                energyBars[i].gameObject.SetActive(true);
            } else {
                energyBars[i].gameObject.SetActive(false);
            }
        }
    }
}