using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] int healAmount = 2;

    protected override void OnPickup(Transform playerRoot)
    {
        PlayerHealth playerHealth = playerRoot.GetComponentInChildren<PlayerHealth>();
        if (playerHealth == null) return;
        playerHealth.AddHealth(healAmount);
    }
}