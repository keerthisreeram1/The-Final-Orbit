using UnityEngine;

public class Weapon : MonoBehaviour
{   
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] LayerMask shootableLayers;

    public void Shoot(WeaponSO weaponSO, Camera mainCamera) {
        if (muzzleFlash) muzzleFlash.Play();
        
        RaycastHit hit;
        
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity, shootableLayers, QueryTriggerInteraction.Ignore)) 
        {
            // Use the hit normal so the sparks/impact face the right way
            Instantiate(weaponSO.HitVFXPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            
            hit.collider.GetComponentInParent<EnemyHealth>()?.TakeDamage(weaponSO.Damage);
        }
    }

}