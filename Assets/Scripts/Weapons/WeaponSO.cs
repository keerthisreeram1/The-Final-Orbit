using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]

public class WeaponSO : ScriptableObject
{
    public GameObject weaponPrefab;
    public int Damage = 1;
    public float FireRate = 0.5f;
    public GameObject HitVFXPrefab;
    public bool IsAutomatic = false;
    public bool CanZoom = false;
    public float ZoomAmount = 10f;
    public float ZoomRotationSpeed = 0.3f;
    public int MaxAmmo = 12;
    public float ReloadTime = 2f;
    
    // IK grip positions relative to ActiveWeapon
    public Vector3 leftHandPosition;
    public Vector3 rightHandPosition;
    public Vector3 leftHandRotation;
    public Vector3 rightHandRotation;
}
