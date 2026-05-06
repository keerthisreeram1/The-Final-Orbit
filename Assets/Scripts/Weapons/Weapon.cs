using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    StarterAssetsInputs starterAssetsInputs;
    Camera mainCamera;

    [SerializeField] int damageAmount = 1;
    [SerializeField] ParticleSystem muzzleFlash;

    private void Awake() {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        mainCamera = Camera.main;
    }

    void Update() {
        HandleShoot();
    }

    void HandleShoot() {
        if (!starterAssetsInputs.shoot) return;

        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity)) {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            enemyHealth?.TakeDamage(damageAmount);
        }

        starterAssetsInputs.ShootInput(false);
    }
}