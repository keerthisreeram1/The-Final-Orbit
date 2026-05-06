using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    StarterAssetsInputs starterAssetsInputs;

    // Projectiles
    [SerializeField] ParticleSystem muzzleFlash;

    private void Awake() {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
    }
    
    void Update() {
        HandleShoot();
    }

    void HandleShoot() {
        if (!starterAssetsInputs.shoot) return;

        // Play muzzleFlash on everyshot
        muzzleFlash.Play();

        RaycastHit hit;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity)){
            Debug.Log(hit.collider.name);
        }
        
        starterAssetsInputs.ShootInput(false);
    }
}
