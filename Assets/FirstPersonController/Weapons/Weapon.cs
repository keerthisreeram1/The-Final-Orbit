using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    [SerializeField] GameObject hitVFXPrefab;
    [SerializeField] Animator animator;
    [SerializeField] int damageAmount = 1;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] LayerMask shootableLayers;

    StarterAssetsInputs starterAssetsInputs;
    Camera mainCamera;

    const string SHOOT_TRIGGER = "shoot";

    private void Awake() {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        mainCamera = Camera.main;
    }

    void Update() {
        HandleShoot();
    }

    void HandleShoot() {
        if (!starterAssetsInputs.shoot) return;
        starterAssetsInputs.ShootInput(false);

        if (muzzleFlash) muzzleFlash.Play();
        if (animator) animator.SetTrigger(SHOOT_TRIGGER);

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity, shootableLayers)) {
            Instantiate(hitVFXPrefab, hit.point, Quaternion.identity);
            hit.collider.GetComponentInParent<EnemyHealth>()?.TakeDamage(damageAmount);
        }
    }
}