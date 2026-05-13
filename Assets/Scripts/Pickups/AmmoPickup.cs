using UnityEngine;

public class AmmoPickup : Pickup
{
    [SerializeField] int ammoAmount = 10;

    protected override void OnPickup(Transform playerRoot)
    {
        ActiveWeapon activeWeapon = playerRoot.GetComponentInChildren<ActiveWeapon>();
        if (activeWeapon == null) return;
        activeWeapon.AddAmmo(ammoAmount);
    }
}