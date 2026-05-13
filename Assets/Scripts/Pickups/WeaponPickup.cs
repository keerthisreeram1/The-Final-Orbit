using UnityEngine;

public class WeaponPickup : Pickup
{
    [SerializeField] WeaponSO weaponSO;

    protected override void OnPickup(Transform playerRoot)
    {
        if (weaponSO == null)
        {
            Debug.LogWarning("WeaponPickup has no WeaponSO assigned.");
            return;
        }

        ActiveWeapon activeWeapon = playerRoot.GetComponentInChildren<ActiveWeapon>();
        if (activeWeapon == null) return;

        activeWeapon.SwitchWeapon(weaponSO);
    }
}