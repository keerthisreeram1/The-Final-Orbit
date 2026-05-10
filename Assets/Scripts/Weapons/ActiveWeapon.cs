using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using StarterAssets;
using Unity.Cinemachine;
using TMPro;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO startingWeaponSO;
    [SerializeField] TwoBoneIKConstraint leftArmIK;
    [SerializeField] TwoBoneIKConstraint rightArmIK;
    [SerializeField] RigBuilder rigBuilder;
    [SerializeField] CinemachineCamera playerFollowCamera;
    [SerializeField] GameObject zoomVignette;
    [SerializeField] TMP_Text ammoText;

    WeaponSO currentWeaponSO;
    Animator animator;
    FirstPersonController firstPersonController;
    StarterAssetsInputs starterAssetsInputs;
    Weapon currentWeapon;
    Camera mainCamera;

    bool isSwitching = false;
    bool isReloading = false;
    float timeSinceLastShot = 0f;
    float defaultFOV;
    float defaultRotationSpeed;
    int currentAmmo;

    const string SHOOT_TRIGGER = "shoot";

    private void Awake() {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        defaultFOV = playerFollowCamera.Lens.FieldOfView;
        firstPersonController = GetComponentInParent<FirstPersonController>();
        defaultRotationSpeed = firstPersonController.RotationSpeed;
    }

    private void Start() {
        SwitchWeapon(startingWeaponSO);
    }

    void Update() {
        HandleShoot();
        HandleZoom();
        HandleReload();
    }

    public void SwitchWeapon(WeaponSO newWeaponSO) {
        StartCoroutine(SwapWeaponRoutine(newWeaponSO));
    }

    public void AddAmmo(int amount) {
        if (currentWeaponSO == null) return;
        currentAmmo = Mathf.Min(currentAmmo + amount, currentWeaponSO.MaxAmmo);
        UpdateAmmoDisplay();
    }

    void UpdateAmmoDisplay() {
        if (ammoText) ammoText.text = $"{currentAmmo}";
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
        currentWeaponSO = newWeaponSO;

        currentAmmo = currentWeaponSO.MaxAmmo;
        UpdateAmmoDisplay();

        ApplyGripPositions(currentWeaponSO);
        AssignIKTargetsAndRebind();
        if (rigBuilder) rigBuilder.Build();

        isSwitching = false;
    }

    IEnumerator ReloadRoutine() {
        isReloading = true;
        yield return new WaitForSeconds(currentWeaponSO.ReloadTime);
        currentAmmo = currentWeaponSO.MaxAmmo;
        UpdateAmmoDisplay();
        isReloading = false;
    }

    void AssignIKTargetsAndRebind() {
        if (currentWeapon == null) return;

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
        timeSinceLastShot += Time.deltaTime;
        if (currentWeaponSO == null) return;
        if (isSwitching || isReloading) return;
        if (starterAssetsInputs == null || currentWeapon == null) return;
        if (!starterAssetsInputs.shoot) return;
        if (currentAmmo <= 0) return;

        if (timeSinceLastShot >= currentWeaponSO.FireRate) {
            currentWeapon.Shoot(currentWeaponSO, mainCamera);
            if (animator) animator.SetTrigger(SHOOT_TRIGGER);
            timeSinceLastShot = 0f;
            currentAmmo--;
            UpdateAmmoDisplay();
        }

        if (!currentWeaponSO.IsAutomatic) {
            starterAssetsInputs.ShootInput(false);
        }
    }

    void HandleReload() {
        if (currentWeaponSO == null) return;
        if (isReloading || isSwitching) return;
        if (currentAmmo == currentWeaponSO.MaxAmmo) return;
        if (!starterAssetsInputs.reload) return;

        StartCoroutine(ReloadRoutine());
    }

    void HandleZoom() {
        if (currentWeaponSO == null) return;
        if (!currentWeaponSO.CanZoom) return;

        var lens = playerFollowCamera.Lens;

        if (starterAssetsInputs.zoom) {
            lens.FieldOfView = currentWeaponSO.ZoomAmount;
            if (zoomVignette) zoomVignette.SetActive(true);
            firstPersonController.ChangeRotationSpeed(currentWeaponSO.ZoomRotationSpeed);
        } else {
            lens.FieldOfView = defaultFOV;
            if (zoomVignette) zoomVignette.SetActive(false);
            firstPersonController.ChangeRotationSpeed(defaultRotationSpeed);
        }

        playerFollowCamera.Lens = lens;
    }
}