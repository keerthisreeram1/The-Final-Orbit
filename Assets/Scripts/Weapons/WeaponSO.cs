using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]

public class WeaponSO : ScriptableObject
{
    public GameObject weaponPrefab;
    public int Damage = 1;
    public float FireRate = 0.5f;
    public GameObject HitVFXPrefab;
    public bool IsAutomatic = false;
    
    // IK grip positions relative to ActiveWeapon
    public Vector3 leftHandPosition;
    public Vector3 rightHandPosition;
    public Vector3 leftHandRotation;
    public Vector3 rightHandRotation;
}
