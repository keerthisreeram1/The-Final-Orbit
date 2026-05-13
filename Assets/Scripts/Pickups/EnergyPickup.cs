using UnityEngine;

public class EnergyPickup : Pickup
{
    [SerializeField] int energyAmount = 2;

    protected override void OnPickup(Transform playerRoot)
    {
        PlayerEnergy playerEnergy = playerRoot.GetComponentInChildren<PlayerEnergy>();
        if (playerEnergy == null) return;
        playerEnergy.AddEnergy(energyAmount);
    }
}