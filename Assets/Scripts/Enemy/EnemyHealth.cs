using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] GameObject robotExplosionVFX;
    [SerializeField] int startingHealth = 3;

    int currentHealth;
    GameManager gameManager;

    void Awake() {
        currentHealth = startingHealth;
    }

    void Start() {
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.AdjustEnemiesLeft(1);
    }

    public void TakeDamage(int amount){
        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (robotExplosionVFX != null)
            Instantiate(robotExplosionVFX, transform.position, Quaternion.identity);
        gameManager.AdjustEnemiesLeft(-1);
        Destroy(this.gameObject);
    }
    
}
