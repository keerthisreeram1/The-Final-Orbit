using UnityEngine;
using StarterAssets;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO weaponSO;
    
    Animator animator;
    StarterAssetsInputs starterAssetsInputs;
    Weapon currentWeapon;
    Camera mainCamera;

    float timeSinceLastShot = 0f;
    const string SHOOT_TRIGGER = "shoot";

    private void Awake() {
        // GetComponentInParent still works fine climbing up through PlayerCameraRoot → PlayerCapsule
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    private void Start() {
        currentWeapon = GetComponentInChildren<Weapon>();
    }

    void Update() {
        timeSinceLastShot += Time.deltaTime;
        HandleShoot();
    }

    public void SwitchWeapon(WeaponSO weaponSO){
        if(currentWeapon){
            Destroy(currentWeapon.gameObject);
        }

        Weapon newWeapon = Instantiate(weaponSO.weaponPrefab, transform).GetComponent<Weapon>();

        // Update with new weapon
        currentWeapon = newWeapon;
        this.weaponSO = weaponSO;
    }

    void HandleShoot() {
        if (starterAssetsInputs == null || currentWeapon == null) return;
        if (!starterAssetsInputs.shoot) return;

        if (timeSinceLastShot >= weaponSO.FireRate) {
            currentWeapon.Shoot(weaponSO, mainCamera);
            if (animator) animator.SetTrigger(SHOOT_TRIGGER);
            
            timeSinceLastShot = 0f; // Reset the clock
        }

        if(!weaponSO.IsAutomatic){
            starterAssetsInputs.ShootInput(false);
        }
    }
}
