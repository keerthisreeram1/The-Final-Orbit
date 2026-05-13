using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
using StarterAssets;

public class PlayerHealth : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] int maxHealth = 10;
    [SerializeField] Image[] healthBars;
    [SerializeField] CinemachineCamera deathVirtualCamera;
    [SerializeField] Transform weaponCamera;
    [SerializeField] GameObject gameOverContainer;

    int currentHealth;
    int deathVirtualCameraPriority = 20;

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
            PlayerGameOver();
        }
    }

    void PlayerGameOver()
    {
        WeaponHolder weaponHolder = GetComponentInChildren<WeaponHolder>();
        if (weaponHolder != null)
            weaponHolder.enabled = false;

        weaponCamera.parent = null;
        deathVirtualCamera.Priority = deathVirtualCameraPriority;
        gameOverContainer.SetActive(true);

        StarterAssetsInputs starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
        if (starterAssetsInputs != null)
            starterAssetsInputs.SetCursorState(false);

        Destroy(this.gameObject);
    }

    public int GetHealthAmount()
    {
        return currentHealth;
    }

    public void AddHealth(int amount)
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