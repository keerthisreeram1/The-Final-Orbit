using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using StarterAssets;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO weaponSO;
    [SerializeField] TwoBoneIKConstraint leftArmIK;
    [SerializeField] TwoBoneIKConstraint rightArmIK;
    [SerializeField] RigBuilder rigBuilder;

    Animator animator;
    StarterAssetsInputs starterAssetsInputs;
    Weapon currentWeapon;
    Camera mainCamera;

    bool isSwitching = false;
    float timeSinceLastShot = 0f;
    const string SHOOT_TRIGGER = "shoot";

    private void Awake() {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    private void Start() {
        currentWeapon = GetComponentInChildren<Weapon>();
        if (currentWeapon != null) AssignIKTargetsAndRebind();
    }

    void Update() {
        timeSinceLastShot += Time.deltaTime;
        HandleShoot();
    }

    public void SwitchWeapon(WeaponSO newWeaponSO) {
        StartCoroutine(SwapWeaponRoutine(newWeaponSO));
    }

    IEnumerator SwapWeaponRoutine(WeaponSO newWeaponSO) {
        isSwitching = true;

        // 1. DISCONNECT THE RIG
        // Setting targets to null tells Burst to stop watching those transforms
        if (leftArmIK) leftArmIK.data.target = null;
        if (rightArmIK) rightArmIK.data.target = null;

        // 2. REBIND THE EMPTY RIG
        // This flushes the old transform "Handles" out of the system
        if (rigBuilder) rigBuilder.Build();
        
        yield return null; // Wait for the "null" state to register

        // 3. CLEANUP AND SPAWN
        if (currentWeapon) Destroy(currentWeapon.gameObject);
        
        GameObject weaponGO = Instantiate(newWeaponSO.weaponPrefab, transform);
        weaponGO.transform.localPosition = Vector3.zero;

        currentWeapon = weaponGO.GetComponent<Weapon>();
        weaponSO = newWeaponSO;

        // 4. ASSIGN NEW TARGETS
        AssignNewTargets();

        // 5. FINAL REBUILD
        // This creates fresh "Handles" for the new weapon's transforms
        if (rigBuilder) rigBuilder.Build();

        if (animator) {
            animator.Update(0f); // Forces the animator to snap to the current frame's pose
        }

        isSwitching = false;
    }

    void AssignNewTargets() {
        if (currentWeapon == null) return;

        Transform leftTarget = FindDeepChild(currentWeapon.transform, "LeftArm_target");
        Transform rightTarget = FindDeepChild(currentWeapon.transform, "RightArm_target");

        if (leftArmIK) leftArmIK.data.target = leftTarget;
        if (rightArmIK) rightArmIK.data.target = rightTarget;
    }

    void AssignIKTargetsAndRebind() {
        if (currentWeapon == null) return;

        Transform leftTarget  = FindDeepChild(currentWeapon.transform, "LeftArm_target");
        Transform rightTarget = FindDeepChild(currentWeapon.transform, "RightArm_target");

        if (leftArmIK  && leftTarget)  leftArmIK.data.target  = leftTarget;
        if (rightArmIK && rightTarget) rightArmIK.data.target = rightTarget;
    }

    Transform FindDeepChild(Transform parent, string name) {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>()) {
            if (child.name == name) return child;
        }
        return null;
    }

    void HandleShoot() {
        if (isSwitching) return;
        if (starterAssetsInputs == null || currentWeapon == null) return;
        if (!starterAssetsInputs.shoot) return;

        if (timeSinceLastShot >= weaponSO.FireRate) {
            currentWeapon.Shoot(weaponSO, mainCamera);
            if (animator) animator.SetTrigger(SHOOT_TRIGGER);
            timeSinceLastShot = 0f;
        }

        if (!weaponSO.IsAutomatic) {
            starterAssetsInputs.ShootInput(false);
        }
    }
}