using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;

    void LateUpdate() {
        transform.rotation = cameraRoot.rotation;
    }
}