using UnityEngine;
using Unity.Cinemachine;

public class Weapon : MonoBehaviour
{   
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] LayerMask shootableLayers;

    CinemachineImpulseSource impulseSource;

    void Awake(){
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shoot(WeaponSO weaponSO, Camera mainCamera) {
        if (muzzleFlash) muzzleFlash.Play();
        if (impulseSource) impulseSource.GenerateImpulse();
        
        RaycastHit hit;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity, shootableLayers, QueryTriggerInteraction.Ignore)) 
        {
            // Use the hit normal so the sparks/impact face the right way
            Instantiate(weaponSO.HitVFXPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            
            hit.collider.GetComponentInParent<EnemyHealth>()?.TakeDamage(weaponSO.Damage);
        }
    }

}