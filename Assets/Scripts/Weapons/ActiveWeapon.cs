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

        if (leftArmIK) leftArmIK.data.target = null;
        if (rightArmIK) rightArmIK.data.target = null;

        yield return null;

        if (currentWeapon) Destroy(currentWeapon.gameObject);

        GameObject weaponGO = Instantiate(newWeaponSO.weaponPrefab, transform);
        weaponGO.transform.localPosition = Vector3.zero;

        currentWeapon = weaponGO.GetComponent<Weapon>();
        weaponSO = newWeaponSO;

        // Apply grip positions from WeaponSO
        ApplyGripPositions(newWeaponSO);

        // Assign IK targets and rebuild
        AssignIKTargetsAndRebind();

        if (rigBuilder) rigBuilder.Build();

        isSwitching = false;
    }

    void AssignIKTargetsAndRebind() {
        if (currentWeapon == null) return;

        // Search ActiveWeapon's own children for the targets
        Transform leftTarget  = FindDeepChild(transform, "LeftArm_target");
        Transform rightTarget = FindDeepChild(transform, "RightArm_target");

        if (leftArmIK  && leftTarget)  leftArmIK.data.target  = leftTarget;
        if (rightArmIK && rightTarget) rightArmIK.data.target = rightTarget;
    }

    void ApplyGripPositions(WeaponSO weaponSO) {
        Transform leftTarget  = FindDeepChild(transform, "LeftArm_target");
        Transform rightTarget = FindDeepChild(transform, "RightArm_target");

        if (leftTarget) {
            leftTarget.localPosition    = weaponSO.leftHandPosition;
            leftTarget.localEulerAngles = weaponSO.leftHandRotation;
        }
        if (rightTarget) {
            rightTarget.localPosition    = weaponSO.rightHandPosition;
            rightTarget.localEulerAngles = weaponSO.rightHandRotation;
        }
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